using Microsoft.AspNetCore.Mvc;

namespace OurWaterAPI
{
    public class Helper
    {
        public static ObjectResult res(object? data = null, string message = "Success", int code = 200)
        {
            return new ObjectResult(new {data, message})
            {
                StatusCode = 200
            };
        }
        public static ObjectResult err(string message, int code = 400)
        {
            return res(null, message, code);
        }

        public static ObjectResult msg(string message = "Success", int code = 200)
        {
            return res(null, message, code);
        }

        public static async Task<string> uploadFile(IFormFile file, string uploadFolder, string? targetName = null)
        {
            var ext = Path.GetExtension(file.FileName);
            var uniqName = targetName ?? $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
            var path = Path.Combine(uploadFolder, uniqName);
            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return uniqName;
        }

    }

    //public class ApiRes
    //{
    //    public string message { get; set; } = "Success";
    //    public object data { get; set; }

    //    public static ApiRes Err(string message)
    //    {
    //        return new ApiRes { message = message };
    //    }
    //}
}
