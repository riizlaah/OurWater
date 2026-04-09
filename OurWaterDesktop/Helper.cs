using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop
{
    public class Helper
    {
        private const string ASSET_URL = "http://localhost:5000/uploads/";
        public static void BindTableColumns(DataGridView table, string[] headers, string[] bindings)
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

        public static async Task<Bitmap> loadImage(string path, HttpClient client)
        {
            try
            {
                var fullUrl = $"{ASSET_URL}{path}";
                Debug.WriteLine(fullUrl);
                var imgBytes = await client.GetByteArrayAsync(fullUrl);
                using (var ms = new System.IO.MemoryStream(imgBytes))
                {
                    var img = Image.FromStream(ms);
                    return new Bitmap(img);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
