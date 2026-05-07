using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using OurWaterAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace OurWaterAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        private readonly IConfiguration conf;
        public UsersController(OurWaterContext ctx, IConfiguration c) { dbc = ctx; conf = c; }

        [HttpPost("login")]
        public ActionResult Login(LoginDTO input)
        {
            var user = dbc.Users.FirstOrDefault(u => u.Username == input.username);
            if (user == null) return Helper.err("User not found");
            if (!sha256Verify(input.password, user.Password))
            {
                return Helper.err("Wrong credentials");
            }
            return Helper.res(new
            {
                fullname = user.Fullname,
                username = user.Username,
                role = user.Role,
                token = GenerateToken(user.Id, user.Role)
            });
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult Me()
        {
            var id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = dbc.Users.Find(id);
            if (user == null) return Helper.err("Not Found", 404);
            return Helper.res(new
            {
                id,
                username = user.Username,
                role = user.Role,
                fullname = user.Fullname,
                address = user.Address
            });
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult GetAll(string? search = null)
        {
            var data = new List<User>();
            if(search != null)
            {
                data = dbc.Users.Where(u => EF.Functions.Like(u.Username, "%" + search + "%") || EF.Functions.Like(u.Fullname, "%" + search + "%")).ToList();
            } else
            {
                data = dbc.Users.ToList();
            }
            return Helper.res(data.Select(u => new
            {
                id = u.Id,
                username = u.Username,
                fullname = u.Fullname,
                role = u.Role,
                address = u.Address,
            }));
        }

        [HttpGet("customers")]
        [Authorize(Roles = "officer")]
        public ActionResult SearchCustomers(string search)
        {
            var data = dbc.Users.Where(u => u.Role == "customer" && ( EF.Functions.Like(u.Username, "%" + search + "%") || EF.Functions.Like(u.Fullname, "%" + search + "%") )).ToList();
            return Helper.res(data.Select(u => new
            {
                id = u.Id,
                username = u.Username,
                fullname = u.Fullname,
                address = u.Address,
            }));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Get(int id)
        {
            var u = dbc.Users.FirstOrDefault(u => u.Id == id);
            if (u == null) return Helper.err("Not Found", 404);
            return Helper.res(new
            {
                id = u.Id,
                username = u.Username,
                fullname = u.Fullname,
                role = u.Role,
                address = u.Address,
            });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(UserDTO input)
        {
            var roles = new[] { "admin", "officer", "customer" };
            if (!roles.Contains(input.role)) return Helper.err("Role not valid");
            if (dbc.Users.Any(u => u.Username == input.username)) return Helper.err("Username has been taken");
            if (input.password.Length < 8) return Helper.err("Password length must be 8 characters or more");
            var user = input.ToEntity();
            user.Password = sha256(user.Password);
            dbc.Users.Add(user);
            dbc.SaveChanges();
            return Helper.msg("User created");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Update(int id, UserDTO input)
        {
            if (!dbc.Users.Any(u => u.Id == id)) return Helper.err("User not found", 404);
            if(dbc.Users.Any(u => u.Username == input.username && u.Id != id))
            {
                return Helper.err("Username has been taken");
            }
            var user = input.ToEntity(id);
            dbc.Users.Attach(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbc.SaveChanges();
            return Helper.msg("User updated");

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            var user = dbc.Users.Find(id);
            if (user == null) return Helper.err("User not found", 404);
            dbc.Users.Remove(user);
            return Helper.msg("User deleted");
        }

        private string sha256(string input)
        {
            using (var obj = SHA256.Create())
            {
                var bytes = obj.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private bool sha256Verify(string input, string hashedStr)
        {
            var hashedInput = sha256(input);
            return StringComparer.OrdinalIgnoreCase.Compare(hashedInput, hashedStr) == 0;
        }

        private string GenerateToken(int id, string role)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, id.ToString()), new Claim(ClaimTypes.Role, role), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(conf["Jwt:Issuer"], conf["Jwt:Audience"], claims, expires: DateTime.Now.AddHours(1), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    public class LoginDTO
    {
        [Required] public string username { get; set; } = null!;
        [Required] public string password { get; set; } = null!;
    }

    public class UserDTO
    {
        [Required] public string username { get; set; } = null!;
        [Required] public string fullname { get; set; } = null!;
        public string password { get; set; } = "";
        [Required] public string role { get; set; } = null!;
        [Required] public string address { get; set; } = null!;

        public User ToEntity()
        {
            return new User { Username = username, Fullname = fullname, Password = password, Role = role, Address = address };
        }
        public User ToEntity(int id)
        {
            return new User { Id = id, Username = username, Fullname = fullname, Password = password, Role = role, Address = address };
        }
    }
}
