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
