using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OurWaterAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OurWaterAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        private readonly IConfiguration conf;
        private readonly IWebHostEnvironment env;

        public AuthController(OurWaterContext ctx, IConfiguration c, IWebHostEnvironment _env)
        {
            dbc = ctx;
            conf = c;
            env = _env;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO input)
        {
            var user = await dbc.Users.FirstOrDefaultAsync(u => u.Username == input.username && u.Password == input.password);
            if (user == null) return Helper.err("Credential invalid");
            return Helper.json(new { token = generateToken(user.Id, user.Role), role = user.Role });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var user = await dbc.Users.AsNoTrackingWithIdentityResolution().FirstAsync(u => u.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Helper.json(new
            {
                id = user.Id,
                username = user.Username,
                fullname = user.Fullname,
                role = user.Role,
                address = user.Address
            });
        }

        private string generateToken(int id, string role)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]));
            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(conf["Jwt:Issuer"], conf["Jwt:Audience"], claims, expires: DateTime.Now.AddHours(1), signingCredentials: signingCreds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginDTO
    {
        [Required] public string username { get; set; } = null!;
        [Required] public string password { get; set; } = null!;
    }
}
