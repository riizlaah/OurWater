using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http.Headers;
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
            if (CurrentSession != null) HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentSession.token);
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

        async public static Task<Bitmap?> FetchImg(string url)
        {
            try
            {
                var actualUrl = $"{addr}uploads/{url}";
                var result = await HttpClient.GetByteArrayAsync(actualUrl);
                using(var ms = new MemoryStream(result))
                {
                    var img = Image.FromStream(ms);
                    return new Bitmap(img);
                }
            } catch(Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
        }

        public static void GenerateTableColumns(DataGridView table, string[] headers, string[] bindings)
        {
            table.AutoGenerateColumns = false;
            for(int i = 0; i < headers.Length; i++)
            {
                var col = new DataGridViewTextBoxColumn();
                col.HeaderText = headers[i];
                col.Name = headers[i];
                col.DataPropertyName = bindings[i];
                col.ReadOnly = true;
                table.Columns.Add(col);
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
