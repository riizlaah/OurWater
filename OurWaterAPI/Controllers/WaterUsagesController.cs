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
        public ActionResult Get(int? year = null)
        {
            var actualYear = year.HasValue ? (year.Value < 1 ? DateTime.Now.Year : year.Value) : DateTime.Now.Year;

            var productions = dbc.ProductionDebitRecords.Where(p => p.Date.Year == actualYear).GroupBy(p => p.Date.Month).Select(p => new
            {
                Month = p.Key,
                Total = p.Sum(rec => rec.Debit)
            }).ToList();
            var consumptions = dbc.ConsumptionDebitRecords.Where(c => c.Date.Year == actualYear && c.Status == "Verified").GroupBy(c => c.Date.Month)
                .Select(c => new
                {
                    Month = c.Key,
                    Total = c.Sum(rec => rec.Debit)
                }).ToList();
            var arr = new List<WaterReport>();
            for(var i = 0; i < consumptions.Count; i++)
            {
                var remainingWaterPercentage = (1m - (consumptions[i].Total / productions[i].Total)) * 100m;
                arr.Add(new WaterReport
                {
                    month = new DateTime(actualYear, consumptions[i].Month, 1).ToString("MMMM"),
                    monthNumber = consumptions[i].Month,
                    totalProdDebit = productions[i].Total,
                    totalConsDebit = consumptions[i].Total,
                    remainingWaterPercentage = remainingWaterPercentage,
                    remainingWater = productions[i].Total - consumptions[i].Total,
                    consDebitPercentage = 100m - remainingWaterPercentage
                });
            }
            return Helper.res(arr);
        }
    }

    public class WaterReport
    {
        public string month { get; set; } = null!;
        public int monthNumber { get; set; }
        public decimal totalProdDebit { get; set; }
        public decimal totalConsDebit { get; set; }
        public decimal remainingWater { get; set; }
        public decimal remainingWaterPercentage { get; set; }
        public decimal consDebitPercentage { get; set; }
    }
}
