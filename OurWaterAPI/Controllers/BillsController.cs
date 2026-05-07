using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurWaterAPI.Models;

namespace OurWaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly OurWaterContext dbc;

        public BillsController(OurWaterContext ctx)
        {
            dbc = ctx;
        }

        [HttpGet]
        [Authorize("admin,customer")]
        public ActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize("admin,customer")]
        public ActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPost("{id}")]
        [Authorize("customer")]
        public ActionResult Pay(int id)
        {
            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize("admin")]
        public ActionResult Patch(int id)
        {
            return Ok();
        }
    }
}
