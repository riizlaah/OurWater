using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurWaterAPI.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace OurWaterAPI.Controllers
{
    [Route("api/bills")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        private readonly string _uploadFolder;
        private readonly IWebHostEnvironment _env;

        public BillsController(OurWaterContext ctx, IWebHostEnvironment env) {
            dbc = ctx;
            _env = env;
            _uploadFolder = Path.Combine(env.ContentRootPath, "wwwroot\\Uploads");
            if (!Directory.Exists(_uploadFolder)) Directory.CreateDirectory(_uploadFolder);
        }

        [HttpGet]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GetAll()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var items = dbc.Bills.ToList();
            var fineRules = dbc.FineRules.ToList();
            items = items.Where(it => it.Status != "Approved").ToList();
            var hasChanged = false;
            foreach (var it in items)
            {
                foreach (var rule in fineRules)
                {
                    if (DateTime.Now.Subtract(it.Deadline).TotalDays >= rule.DayAfterDeadline)
                    {
                        if (dbc.Fines.Any(f => f.FineRuleId == rule.Id && f.BillId == it.Id)) continue;
                        dbc.Fines.Add(new Fine { BillId = it.Id, FineRuleId = rule.Id, CreatedAt = DateTime.Now });
                        hasChanged = true;
                    }
                }
            }
            if (hasChanged) dbc.SaveChanges();
            var data = await dbc.Bills.Include(b => b.ConsumptionRecord).Include(b => b.Fines).ThenInclude(f => f.FineRule).Where(b => b.CustomerId == userId).ToListAsync();
            return Helper.json(data.Select(b => new
            {
                id = b.Id,
                customerId = b.CustomerId,
                consumptionRecord = new
                {
                    id = b.ConsumptionRecordId,
                    debit = b.ConsumptionRecord.Debit,
                    date = b.ConsumptionRecord.Date,
                },
                status = b.Status,
                amount = b.Amount + b.Fines.Sum(f => f.FineRule.FineAmount),
                deadline = b.Deadline,
                updatedAt = b.UpdatedAt,
                createdAt = b.CreatedAt
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var b = dbc.Bills.AsNoTracking().Include(bill => bill.ConsumptionRecord).ThenInclude(c => c.InputtingUser)
                .Include(bill => bill.Fines).ThenInclude(f => f.FineRule).FirstOrDefault(bill => bill.Id == id);
            if (b == null) return Helper.err("Bill not found");
            return Helper.json(new
            {
                id = b.Id,
                customerId = b.CustomerId,
                consumptionRecord = new
                {
                    id = b.ConsumptionRecordId,
                    debit = b.ConsumptionRecord.Debit,
                    date = b.ConsumptionRecord.Date,
                    inputtedBy = b.ConsumptionRecord.InputtingUser.Fullname,
                },
                status = b.Status,
                originalAmount = b.Amount,
                fines = b.Fines.Select(f => $"{f.FineRule.DayAfterDeadline} day x {f.FineRule.FineAmount:Rp#,##0;(Rp#,##0);Rp0}"),
                totalAmount = b.calculateTotal(),
                fineAmount = b.calculateFines(),
                rejectionReason = b.RejectionReason,
                imagePath = b.ImagePath,
                deadline = b.Deadline,
                updatedAt = b.UpdatedAt,
                createdAt = b.CreatedAt
            });
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Store(int id, IFormFile img)
        {
            if (img == null || img.Length == 0) return Helper.err("All field is required");
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var allowedType = new[] { "image/png", "image/jpeg", "image/jpg" };
            if (!allowedType.Contains(img.ContentType)) return Helper.err("Not an image (png/jpg/jpeg) file");
            var bill = dbc.Bills.Find(id);
            if (bill == null) return Helper.err("Bill not found", 404);
            bill.ImagePath = await Helper.UploadFile(img, _uploadFolder, bill.ImagePath);
            bill.Status = "Paid Unconfirmed";
            bill.UpdatedAt = DateTime.Now;
            await dbc.SaveChangesAsync();
            return Helper.json(null);
        }
    }
}
