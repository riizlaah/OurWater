using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurWaterAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace OurWaterAPI.Controllers
{
    [Route("api/debit-record")]
    [ApiController]
    public class ConsumptionDebitRecordController : ControllerBase
    {
        private readonly OurWaterContext dbc;
        private readonly string _uploadFolder;
        private readonly IWebHostEnvironment _env;

        public ConsumptionDebitRecordController(OurWaterContext ctx, IWebHostEnvironment env) {
            dbc = ctx; 
            _env = env;
            _uploadFolder = Path.Combine(env.ContentRootPath, "wwwroot\\Uploads");
            if(!Directory.Exists(_uploadFolder)) Directory.CreateDirectory(_uploadFolder);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var data = new List<ConsumptionDebitRecord>();
            var query = dbc.ConsumptionDebitRecords.AsQueryable().Include(c => c.Customer).Include(c => c.CorrectingUser).Include(c => c.InputtingUser);

            if(role == "officer")
            {
                data = await query.OrderBy(c => c.InputtedBy == userId ? 0 : (c.CorrectedBy == userId ? 1 : 2)).ToListAsync();
            } else
            {
                data = await query.Where(c => c.InputtedBy == userId || c.CustomerId == userId).ToListAsync();
            }
            return Helper.json(data.Select(c => new
            {
                id = c.Id,
                customer = new
                {
                    id = c.CustomerId,
                    name = c.Customer.Fullname
                },
                inputtedBy = new
                {
                    id = c.InputtedBy,
                    name = c.InputtingUser.Fullname
                },
                correctedBy = c.CorrectedBy != null ? new
                {
                    id = c.CorrectedBy,
                    name = c.CorrectingUser.Fullname
                } : null,
                debit = c.Debit,
                date = c.Date,
                status = c.Status,
                location = c.Location,
                updatedAt = c.UpdatedAt
            }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var c = dbc.ConsumptionDebitRecords.Include(c => c.Customer).Include(c => c.CorrectingUser).Include(c => c.InputtingUser).AsNoTracking().First(c => c.Id == id);
            return Helper.json(new
            {
                id = c.Id,
                customer = new
                {
                    id = c.CustomerId,
                    name = c.Customer.Fullname
                },
                inputtedBy = new
                {
                    id = c.InputtedBy,
                    name = c.InputtingUser.Fullname
                },
                correctedBy = c.CorrectedBy != null ? new
                {
                    id = c.CorrectedBy,
                    name = c.CorrectingUser.Fullname
                } : null,
                debit = c.Debit,
                date = c.Date,
                status = c.Status,
                rejectionReason = c.RejectionReason,
                imagePath = c.ImagePath,
                location = c.Location,
                updatedAt = c.UpdatedAt,
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Store(IFormFile img, [FromForm] string location, [FromForm] string customerId, [FromForm] string debit)
        {
            var allowedDay = new[] { 1, 2, 3, 4, 5, 6, 7, 27, 26, 28, 29, 30, 31 };
            //if (!allowedDay.Contains(DateTime.Now.Day)) return Helper.err("Today is not time to input consumption debit record");
            if (location.Trim() == "" || customerId.Trim() == "" || debit.Trim() == "" || (img == null || img.Length == 0) ) return Helper.err("All field is required");
            if (!int.TryParse(customerId, out int custId)) return Helper.err("Customer Id not valid");
            if (!decimal.TryParse(debit, out decimal debitDec)) return Helper.err("Debit not valid");
            if(debitDec < 0.00001m) return Helper.err("Debit not valid");
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "customer";
            var rec = dbc.ConsumptionDebitRecords.Where(c => c.CustomerId == custId && c.Date.Month == DateTime.Now.Month).FirstOrDefault();
            var allowedType = new[] { "image/png", "image/jpeg", "image/jpg" };
            if (!allowedType.Contains(img.ContentType)) return Helper.err("Not an image (png/jpg/jpeg) file");
            if(rec != null)
            {
                rec.Debit = debitDec;
                rec.Location = location;
                rec.Status = "Pending";
                rec.ImagePath = await Helper.UploadFile(img, _uploadFolder);
            } else
            {
                rec = new ConsumptionDebitRecord
                {
                    Debit = debitDec,
                    Location = location,
                    Status = "Pending",
                    CustomerId = custId,
                    InputtedBy = userId,
                    RejectionReason = "",
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    UpdatedAt = DateTime.Now,
                    ImagePath = await Helper.UploadFile(img, _uploadFolder)
                };
                dbc.ConsumptionDebitRecords.Add(rec);
            }
            await dbc.SaveChangesAsync();
            return Helper.json(null);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "officer")]
        public async Task<IActionResult> Patch(int id, ReviewDebitConsDTO input)
        {
            var rec = dbc.ConsumptionDebitRecords.Find(id);
            if (rec == null) return Helper.err("Record not found", 404);
            rec.RejectionReason = input.rejectionReason;
            rec.Status = input.status;
            await dbc.SaveChangesAsync();
            return Helper.json(null);
        }

    }

    public class ReviewDebitConsDTO
    {
        [Required] public string rejectionReason { get; set; } = null!;
        [Required] public string status { get; set; } = null!;
    }
}
