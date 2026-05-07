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
    public class ConsumptionDebitsController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        private readonly string uploadPath;
        public ConsumptionDebitsController(OurWaterContext ctx, IWebHostEnvironment e) 
        { 
            dbc = ctx;
            uploadPath = Path.Combine(e.ContentRootPath, "wwwroot\\Uploads");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
        }

        [HttpPost]
        [Authorize(Roles = "officer,customer")]
        public async Task<ActionResult> Submit(IFormFile img, [FromForm] int customerId, [FromForm] decimal debit)
        {
            var allowedDay = new[] { 1, 2, 3, 4, 5, 6, 7, 26, 27, 28, 29, 30, 31 };
            if (!allowedDay.Contains(DateTime.Now.Day)) return Helper.err("TOday is not a time to input consumption debit");
            if (img == null || img.Length == 0) return Helper.err("Image is required");
            if (debit < 0.000001m) return Helper.err("Debit not valid");
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var customer = dbc.Users.AsNoTracking().FirstOrDefault(u => u.Id == customerId);
            if (customer == null) return Helper.err("Customer not found");
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var rec = dbc.ConsumptionDebitRecords.FirstOrDefault(c => c.CustomerId == customerId && c.Date.Month == DateTime.Now.Month);
            var allowedImg = new[] { "image/png", "image/jpg", "image/jpeg" };
            if (!allowedImg.Contains(img.ContentType)) return Helper.err("The only allowed images are jpg/png");
            if(rec != null)
            {
                if (userId == customerId) return Helper.err("Customer can't correcting the submitted debit record");
                rec.Debit = debit;
                rec.CorrectedBy = userId;
                rec.ImagePath = await Helper.uploadFile(img, uploadPath, rec.ImagePath);
                rec.Status = "Pending";
                rec.UpdatedAt = DateTime.Now;
            } else
            {
                rec = new ConsumptionDebitRecord
                {
                    Location = customer.Address,
                    Debit = debit,
                    Status = "Pending",
                    CustomerId = customerId,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    ImagePath = await Helper.uploadFile(img, uploadPath),
                    InputtedBy = userId,
                };
                dbc.ConsumptionDebitRecords.Add(rec);
            }
            await dbc.SaveChangesAsync();
            return Helper.msg();
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAll()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var query = dbc.ConsumptionDebitRecords.AsNoTracking().AsQueryable().Include(c => c.Customer).Include(c => c.Creator).Include(c => c.Corrector).AsQueryable();
            if(role == "officer")
            {
                query = query.OrderBy(c => c.InputtedBy == userId ? 0 : (c.CorrectedBy == userId ? 1 : 2));
            } else
            {
                query = query.Where(c => c.InputtedBy == userId || c.CustomerId == userId);
            }
            return Helper.res(query.ToList().Select(c => new
            {
                id = c.Id,
                customerName = c.Customer.Fullname,
                inputtedBy = c.Creator.Fullname,
                correctedBy = c.Corrector?.Fullname,
                debit = c.Debit,
                date = c.Date,
                status = c.Status,
                location = c.Location,
                updatedAt = c.UpdatedAt
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult Get(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var c = dbc.ConsumptionDebitRecords.Include(c => c.Creator).Include(c => c.Corrector).Include(c => c.Customer).FirstOrDefault(cdr => cdr.Id == id);
            if (c == null) return Helper.err("Not found", 404);
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            if (role == "customer" && c.CustomerId != userId) return Helper.err("Forbidden", 403);
            return Helper.res(new
            {
                id = c.Id,
                customerName = c.Customer.Fullname,
                inputtedBy = c.Creator.Fullname,
                correctedBy = c.Corrector?.Fullname,
                debit = c.Debit,
                date = c.Date,
                status = c.Status,
                location = c.Location,
                updatedAt = c.UpdatedAt,
                imagePath = c.ImagePath,
            });

        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "admin,officer")]
        public ActionResult Patch(int id, PatchConsumptionDebitRecord input)
        {
            var rec = dbc.ConsumptionDebitRecords.Include(c => c.Creator).Include(c => c.Corrector).FirstOrDefault(c => c.Id == id);
            if (rec == null) return Helper.err("Not found", 404);
            var recUser = rec.Corrector ?? rec.Creator;
            var role = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (role == "officer" && recUser.Role == "officer") return Helper.err("Officer can't review other officer inputted record");
            if (input.status == "Rejected" && input.rejectionReason.Trim() == "") return Helper.err("Rejection reason required");
            rec.Status = input.status;
            rec.RejectionReason = input.rejectionReason;
            rec.UpdatedAt = DateTime.Now;
            if(input.status == "Verified")
            {
                dbc.Bills.Add(new Bill
                {
                    ConsumptionRecordId = rec.Id,
                    CustomerId = rec.CustomerId,
                    Deadline = DateTime.Now.AddDays(14),
                    Status = "Pending",
                    Amount = CalculateBillAmount(rec.Debit),
                });
            }
            dbc.SaveChanges();
            return Helper.msg();
        }

        private decimal CalculateBillAmount(decimal debit)
        {
            var amount = 0m;
            if(debit < 10m)
            {
                amount = debit * 2500m;
            } else if(debit < 20m)
            {
                amount = 25000m + (debit - 10m) * 3500m;
            } else
            {
                amount = 25000m + 35000m + (debit - 20m) * 4000m;
            }
            return amount;
        }
    }

    public class PatchConsumptionDebitRecord
    {
        [Required(AllowEmptyStrings = true)] public string rejectionReason { get; set; } = "";
        [Required] public string status { get; set; } = null!;
    }
}
