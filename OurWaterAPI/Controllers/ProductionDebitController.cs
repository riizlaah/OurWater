using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurWaterAPI.Models;

namespace OurWaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionDebitsController : ControllerBase
    {
        private readonly OurWaterContext dbc;

        public ProductionDebitsController(OurWaterContext ctx)
        {
            dbc = ctx;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Submit()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult GetAll()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Update(int id)
        {
            return Ok();
        }
    }
}
