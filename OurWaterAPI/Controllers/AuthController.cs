using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        private readonly IConfiguration conf;
        public AuthController(OurWaterContext ctx, IConfiguration c) { dbc = ctx; conf = c; }

        [HttpPost("login")]
        public ActionResult Login(LoginDTO input)
        {
            var user = dbc.Users.FirstOrDefault(u => u.Username == input.username);
            if (user == null) return Helper.err("User not found");
            if (!sha256Verify(input.password, user.Password))
            {
                return Helper.err("Password not valid");
            }
            return Helper.res(new
            {
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
                id = id,
                username = user.Username,
                role = user.Role,
                fullname = user.Fullname,
                address = user.Address
            });
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
}
