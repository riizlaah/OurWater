using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurWaterAPI.Models;
using System.ComponentModel.DataAnnotations;

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
            var data = dbc.FineRules.ToList();
            return Helper.res(data.Select(fr => new
            {
                id = fr.Id,
                startDay = fr.StartDay,
                endDay = fr.EndDay,
                amount = fr.FineAmount
            }));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Get(int id)
        {
            var fr = dbc.FineRules.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (fr == null) Helper.err("Not found", 404);
            return Helper.res(new
            {
                id = fr.Id,
                startDay = fr.StartDay,
                endDay = fr.EndDay,
                amount = fr.FineAmount
            });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(FineRuleDTO input)
        {
            if (input.amount <= 0m) return Helper.err("Amount not valid");
            if (input.startDay < 1) return Helper.err("Start day not valid");
            if (input.endDay.HasValue ? input.endDay.Value < 1 : false) return Helper.err("End day not valid");
            if (dbc.FineRules.Any(fr => fr.EndDay == null) && input.endDay == null) return Helper.err("Continuous rule can't exist multiple time");
            if(dbc.FineRules.Any(fr => input.startDay <= (fr.EndDay ?? 0) || (input.endDay.HasValue ? input.endDay.Value <= fr.StartDay : false) || (fr.EndDay == null && input.startDay > 0)))
            {
                return Helper.err("Day range collided with other fine rule");
            }
            dbc.FineRules.Add(input.ToEntity());
            dbc.SaveChanges();
            return Helper.msg();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Update(int id, FineRuleDTO input)
        {
            if (input.amount <= 0m) return Helper.err("Amount not valid");
            if (input.startDay < 1) return Helper.err("Start day not valid");
            if (input.endDay.HasValue ? input.endDay.Value < 1 : false) return Helper.err("End day not valid");
            if (dbc.FineRules.Any(fr => fr.EndDay == null && fr.Id != id) && input.endDay == null) return Helper.err("Continuous rule can't exist multiple time");
            if (dbc.FineRules.Any(fr => fr.Id != id && (input.startDay <= (fr.EndDay ?? 0) || (input.endDay.HasValue ? input.endDay.Value <= fr.StartDay : false) || (fr.EndDay == null && input.startDay > 0))))
            {
                return Helper.err("Day range collided with other fine rule");
            }
            dbc.FineRules.Attach(input.ToEntity(id)).State = EntityState.Modified;
            dbc.SaveChanges();
            return Helper.msg();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            var fr = dbc.FineRules.Find(id);
            if (fr == null) return Helper.err("Not found", 404);
            dbc.FineRules.Remove(fr);
            dbc.SaveChanges();
            return Helper.msg();
        }
    }

    public class FineRuleDTO
    {
        [Required] public int startDay { get; set; }
        public int? endDay { get; set; } = null;
        [Required] public decimal amount { get; set; }

        public FineRule ToEntity()
        {
            return new FineRule { StartDay = startDay, EndDay = endDay, FineAmount = amount };
        }

        public FineRule ToEntity(int id)
        {
            return new FineRule { Id = id, StartDay = startDay, EndDay = endDay, FineAmount = amount };
        }
    }
}
