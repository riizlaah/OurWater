using Microsoft.AspNetCore.Mvc;

namespace OurWaterAPI
{
    public class Helper
    {
        public static ObjectResult json(object? obj, string msg = "Success", int code = 200)
        {
            return new ObjectResult(new { data = obj, message = msg })
            {
                StatusCode = code
            };
        }

        public static ObjectResult err(string msg, int code = 400)
        {
            return json(null, msg, code);
        }

        public static async Task<string> UploadFile(IFormFile file, string uploadFolder)
        {
            var ext = Path.GetExtension(file.FileName);
            var uniqName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
            var path = Path.Combine(uploadFolder, uniqName);
            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return uniqName;
        } 
    }
}
