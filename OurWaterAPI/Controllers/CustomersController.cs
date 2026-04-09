using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OurWaterAPI.Models;

namespace OurWaterAPI.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        public CustomersController(OurWaterContext ctx) { dbc = ctx; }

        [HttpGet]
        [Authorize(Roles = "officer")]
        public async Task<IActionResult> Search(string s)
        {
            var data = dbc.Users.Where(u => u.Role == "customer" && (EF.Functions.Like(u.Fullname, "%" + s + "%") || u.Id.ToString().Contains(s)));
            return Helper.json(data.Select(u => new
            {
                id = u.Id,
                name = u.Fullname,
            }));
        }

    }
}
