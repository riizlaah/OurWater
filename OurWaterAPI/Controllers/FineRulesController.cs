using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurWaterAPI.Models;

namespace OurWaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FineRulesController : ControllerBase
    {
        private readonly OurWaterContext dbc;

        public FineRulesController(OurWaterContext ctx)
        {
            dbc = ctx;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Update()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete()
        {
            return Ok();
        }
    }
}
