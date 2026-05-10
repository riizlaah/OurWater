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
            if (dbc.ProductionDebitRecords.Any(p => p.Date == input.date)) return Helper.err("Record already exists");
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userLocation = dbc.Users.Where(u => u.Id == userId).Select(u => u.Address).First();
            dbc.ProductionDebitRecords.Add(new ProductionDebitRecord
            {
                Debit = input.debit,
                Date = input.date,
                InputtedBy = userId,
                Location = userLocation,
            });
            dbc.SaveChanges();
            return Helper.msg();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult GetAll(int? month = null, int? year = null)
        {
            var actualMonth = month != null ? (month.Value <= 12 && month.Value > 0 ? month.Value : DateTime.Now.Month) : DateTime.Now.Month;
            var actualYear = year != null ? (year.Value < 1 ? DateTime.Now.Year : year.Value) : DateTime.Now.Year;
            var data = dbc.ProductionDebitRecords.Include(p => p.Creator).Where(p => p.Date.Year == actualYear && p.Date.Month == actualMonth).ToList();
            return Helper.res(data.Select(p => new
            {
                id = p.Id,
                debit = p.Debit,
                date = p.Date,
                inputtedBy = p.Creator.Fullname,
            }));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Update(int id, ProdDebitDTO input)
        {
            var rec = dbc.ProductionDebitRecords.Find(id);
            if (rec == null) return Helper.err("Not found", 404);
            rec.Debit = input.debit;
            dbc.SaveChanges();
            return Helper.msg();
        }
    }

    public class ProdDebitDTO
    {
        [Required] public decimal debit { get; set; }
        [Required] public DateOnly date { get; set; }
    }
}
