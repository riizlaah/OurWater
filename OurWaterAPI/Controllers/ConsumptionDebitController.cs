using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurWaterAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
            //if (!allowedDay.Contains(DateTime.Now.Day)) return Helper.err("Today is not a time to input consumption debit");
            if (img == null || img.Length == 0) return Helper.err("Image is required");
            if (debit <= 0m) return Helper.err("Debit not valid");
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var customer = await dbc.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == customerId);
            if (customer == null) return Helper.err("Customer not found");
            if (customer.Role != "customer") return Helper.err("Not a customer");
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var rec = await dbc.ConsumptionDebitRecords.FirstOrDefaultAsync(c => c.CustomerId == customerId && c.Date.Month == DateTime.Today.Month && c.Date.Year == DateTime.Today.Year);
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
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    ImagePath = await Helper.uploadFile(img, uploadPath),
                    InputtedBy = userId,
                    RejectionReason = "",
                    UpdatedAt = DateTime.Now
                };
                dbc.ConsumptionDebitRecords.Add(rec);
            }
            await dbc.SaveChangesAsync();
            return Helper.msg();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "officer")]
        async public Task<ActionResult> Update(int id, IFormFile img, [FromForm] int customerId, [FromForm] decimal debit)
        {
            var allowedDay = new[] { 1, 2, 3, 4, 5, 6, 7, 26, 27, 28, 29, 30, 31 };
            //if (!allowedDay.Contains(DateTime.Now.Day)) return Helper.err("Today is not a time to input consumption debit");
            if (img == null || img.Length == 0) return Helper.err("Image is required");
            var allowedImg = new[] { "image/png", "image/jpeg" };
            if (!allowedImg.Contains(img.ContentType)) return Helper.err("The only allowed images are jpg/png");
            if (debit <= 0m) return Helper.err("Debit not valid");
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var customer = await dbc.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == customerId);
            if (customer == null) return Helper.err("Customer not found");
            if (customer.Role != "customer") return Helper.err("Not a customer");
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var rec = await dbc.ConsumptionDebitRecords.FindAsync(id);
            if (rec == null) return Helper.err("Not found", 404);
            if (userId == customerId) return Helper.err("Customer can't correcting the submitted debit record");
            rec.Debit = debit;
            rec.CorrectedBy = userId;
            rec.CustomerId = customerId;
            rec.ImagePath = await Helper.uploadFile(img, uploadPath, rec.ImagePath);
            rec.Status = "Pending";
            rec.UpdatedAt = DateTime.Now;
            await dbc.SaveChangesAsync();
            
            return Helper.msg();
        }

        [HttpGet("customer/{id}")]
        [Authorize(Roles = "officer")]
        public ActionResult GetByCustomerId(int id)
        {
            var currYear = DateTime.Today.Year;
            var currMonth = DateTime.Today.Month;
            var rec = dbc.ConsumptionDebitRecords.Include(c => c.Customer).FirstOrDefault(c => c.CustomerId == id && c.Date.Year == currYear && c.Date.Month == currMonth);
            if (rec == null) return Helper.err("Not found", 404);
            return Helper.res(new
            {
                id = rec.Id,
                debit = rec.Debit,
                imagePath = rec.ImagePath,
                status = rec.Status,
                rejectionReason = rec.RejectionReason,
                customer = new
                {
                    id = rec.CustomerId,
                    name = rec.Customer.Fullname,
                    address = rec.Customer.Address
                }
            });
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
            } else if(role == "customer")
            {
                query = query.Where(c => c.InputtedBy == userId || c.CustomerId == userId);
            }
            return Helper.res(query.OrderByDescending(b => b.UpdatedAt).ToList().Select(c => new
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
            var prevMonth = c.Date.Month == 1 ? 12 : c.Date.Month - 1;
            var prevYear = prevMonth == 1 ? c.Date.Year - 1 : c.Date.Year;
            var prevDebit = dbc.ConsumptionDebitRecords.FirstOrDefault(cd => cd.CustomerId == c.CustomerId && cd.Date.Month == prevMonth && cd.Date.Year == prevYear);
            return Helper.res(new
            {
                id = c.Id,
                customerName = c.Customer.Fullname,
                inputtedBy = c.Creator.Fullname,
                correctedBy = c.Corrector?.Fullname,
                debit = c.Debit,
                prevDebit = prevDebit?.Debit,
                date = c.Date,
                status = c.Status,
                location = c.Location,
                updatedAt = c.UpdatedAt,
                imagePath = c.ImagePath,
                rejectionReason = c.RejectionReason,
            });

        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "admin,officer")]
        public ActionResult Patch(int id, PatchConsumptionDebitRecord input)
        {
            var allowed = new[] { "Verified", "Rejected" };
            if (!allowed.Contains(input.status)) return Helper.err("Status not valid");
            var rec = dbc.ConsumptionDebitRecords.Include(c => c.Creator).Include(c => c.Corrector).FirstOrDefault(c => c.Id == id);
            if (rec == null) return Helper.err("Not found", 404);
            //if (rec.Status != "Pending") return Helper.err("Status can't be changed");
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
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    RejectionReason = "",
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
