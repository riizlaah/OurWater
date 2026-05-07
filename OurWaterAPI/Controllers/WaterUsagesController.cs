using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurWaterAPI.Models;

namespace OurWaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterUsagesController : ControllerBase
    {
        private readonly OurWaterContext dbc;

        public WaterUsagesController(OurWaterContext ctx)
        {
            dbc = ctx;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
