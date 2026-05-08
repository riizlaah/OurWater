using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OurWaterDesktop
{
    public class Helper
    {
        private const string addr = "http://localhost:5000/";
        public static HttpClient HttpClient { get; } = new HttpClient();

        public static LoginRes? CurrentSession { get; set; }

        public async static Task<(bool isSuccess, ApiResponse<TRes> result)> JsonReq<TReq, TRes>(string route, TReq? req = default, string method = "get") where TRes : class
        {
            HttpResponseMessage res;
            var url = $"{addr}api/{route}";
            method = method.ToLower();
            if (method == "get")
            {
                res = await HttpClient.GetAsync(url);
            } else if(method == "post")
            {
                res = await HttpClient.PostAsJsonAsync(url, req);
            } else if(method == "put")
            {
                res = await HttpClient.PutAsJsonAsync(url, req);
            } else if (method == "patch")
            {
                res = await HttpClient.PatchAsJsonAsync(url, req);
            } else
            {
                res = await HttpClient.DeleteAsync(url);
            }
            try
            {
                var res2 = await res.Content.ReadFromJsonAsync<ApiResponse<TRes>>();
                if (res2 == null)
                {
                    return (false, ApiResponse<TRes>.Error("Network error"));
                }
                return (res.IsSuccessStatusCode, res2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return (false, ApiResponse<TRes>.Error(ex.Message));
            }
        }
    }

    public class ApiResponse<T> where T : class
    {
        public T? data { get; set; }
        public string message { get; set; }



        public static ApiResponse<T> Error(string msg)
        {
            return new ApiResponse<T> { data = null, message = msg };
        }
    }
}
