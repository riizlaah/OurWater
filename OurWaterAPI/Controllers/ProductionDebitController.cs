using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurWaterAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
        public ActionResult Submit(ProdDebitDTO input)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            dbc.ProductionDebitRecords.Add(new ProductionDebitRecord
            {
                Debit = input.debit,
                Date = input.date,
                InputtedBy = userId,
                Location = input.location,
            });
            dbc.SaveChanges();
            return Helper.msg();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult GetAll(int? month = null, int? year = null)
        {
            var actualMonth = month != null ? (month.Value <= 12 && month.Value > 0 ? month.Value : DateTime.Now.Month) : DateTime.Now.Month;
            var actualYear = month != null ? (month.Value < 1 ? DateTime.Now.Year : month.Value) : DateTime.Now.Year;
            var data = dbc.ProductionDebitRecords.Include(p => p.Creator).Where(p => p.Date.Year == actualYear && p.Date.Month == actualMonth).ToList();
            return Helper.res(data.Select(p => new
            {
                id = p.Id,
                debit = p.Debit,
                date = p.Date,
                inputtedBy = p.Creator.Fullname,
                //location = p.Location
            }));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Update(int id, ProdDebitDTO input)
        {
            var rec = dbc.ProductionDebitRecords.Find(id);
            if (rec == null) return Helper.err("Not found", 404);
            rec.Debit = input.debit;
            rec.Date = input.date;
            return Helper.msg();
        }
    }

    public class ProdDebitDTO
    {
        [Required] public decimal debit { get; set; }
        [Required] public DateOnly date { get; set; }
        [Required] public string location { get; set; } = "";
    }
}
