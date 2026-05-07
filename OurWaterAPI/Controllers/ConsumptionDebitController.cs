using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurWaterAPI.Models;

namespace OurWaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumptionDebitsController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        public ConsumptionDebitsController(OurWaterContext ctx) { dbc = ctx; }

        [HttpPost]
        [Authorize]
        public ActionResult Submit()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Patch(int id)
        {
            return Ok();
        }
    }
}
