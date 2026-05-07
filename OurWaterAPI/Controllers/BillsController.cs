using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurWaterAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace OurWaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        private readonly string uploadPath;

        public BillsController(OurWaterContext ctx, IWebHostEnvironment e)
        {
            dbc = ctx;
            uploadPath = Path.Combine(e.ContentRootPath, "wwwroot\\Uploads");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
        }

        [HttpGet]
        [Authorize(Roles = "admin,customer")]
        public ActionResult GetAll()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //var user = dbc.Users.FirstOrDefault(u => u.Id == userId);
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var fineRules = dbc.FineRules.ToList();
            var bills = dbc.Bills.Include(b => b.Customer).Include(b => b.Fines).ThenInclude(f => f.FineRule).AsQueryable();
            var now = DateTime.Now;
            if(role == "customer")
            {
                bills = bills.Where(b => b.CustomerId == userId);
            }
            var bills2 = bills.ToList();
            var somethingChanged = false;
            foreach(var bill in bills2)
            {
                var totalDays = (now - bill.CreatedAt).TotalDays;
                var fineRuleIds = bill.Fines.Select(f => f.Id).ToList();
                var fr = fineRules.FirstOrDefault(f => !fineRuleIds.Contains(f.Id) && (f.EndDay.HasValue ? f.StartDay <= totalDays && totalDays <= f.EndDay.Value : f.StartDay <= totalDays));
                if (fr == null) continue;
                bill.Fines.Add(new Fine { BillId = bill.Id, FineRuleId = fr.Id });
                somethingChanged = true;
            }
            if(somethingChanged) dbc.SaveChanges();
            return Helper.res(bills2.Select(b => new
            {
                id = b.Id,
                consumptionDebitRecord = new
                {
                    id = b.ConsumptionRecordId,
                    debit = b.ConsumptionRecord.Debit
                },
                customer = new
                {
                    name = b.Customer.Fullname,
                },
                originalAmount = b.Amount,
                extraFine = b.ExtraFine,
                totalAmount = b.TotalPrice,
                deadline = b.Deadline,
                status = b.Status,
                createdAt = b.CreatedAt
            }));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,customer")]
        public ActionResult Get(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var b = dbc.Bills.Include(b => b.Customer).Include(b => b.Fines).ThenInclude(b => b.FineRule).FirstOrDefault(b => b.Id == id);
            if (b == null) return Helper.err("Not found", 404);
            if (role == "customer" && b.CustomerId != userId) return Helper.err("Forbidden", 403);
            return Helper.res(new
            {
                id = b.Id,
                consumptionDebitRecord = new
                {
                    id = b.ConsumptionRecordId,
                    debit = b.ConsumptionRecord.Debit
                },
                customer = new
                {
                    name = b.Customer.Fullname,
                    address = b.Customer.Address,
                },
                originalAmount = b.Amount,
                extraFine = b.ExtraFine,
                fines = b.Fines.Select(f => f.FineRule.DisplayStr).ToArray(),
                totalAmount = b.TotalPrice,
                deadline = b.Deadline,
                status = b.Status,
                rejectionReason = b.RejectionReason,
                imagePath = b.ImagePath,
                createdAt = b.CreatedAt
            });
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> Pay(int id, IFormFile img)
        {
            if (img == null || img.Length == 0) return Helper.err("Image is required");
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var b = dbc.Bills.Include(b => b.Customer).FirstOrDefault(b => b.Id == id);
            if (b == null) return Helper.err("Not found", 404);
            if (b.CustomerId != userId) return Helper.err("Forbidden", 403);
            var allowedImg = new[] { "image/png", "image/jpg", "image/jpeg" };
            if (!allowedImg.Contains(img.ContentType)) return Helper.err("The only allowed images are jpg/png");
            b.ImagePath = await Helper.uploadFile(img, uploadPath, b.ImagePath);
            b.Status = "Paid Unconfirmed";
            b.UpdatedAt = DateTime.Now;
            await dbc.SaveChangesAsync();
            return Helper.msg();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Patch(int id, PatchBillDTO input)
        {
            var allowedStatus = new[] {"Verified", "Rejected"};
            if (!allowedStatus.Contains(input.status)) return Helper.err("Status not valid");
            if (input.status == "Rejected" && input.rejectionReason.Trim() == "") return Helper.err("Rejection Reason required");
            var b = dbc.Bills.Find(id);
            if (b == null) return Helper.err("Not found", 404);
            b.Status = input.status;
            b.RejectionReason = input.rejectionReason;
            dbc.SaveChanges();
            return Helper.msg();
        }
    }

    public class PatchBillDTO
    {
        [Required] public string status { get; set; } = null!;
        [Required(AllowEmptyStrings = true)] public string rejectionReason { get; set; } = "";

    }
}
