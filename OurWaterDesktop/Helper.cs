using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop
{
    public class Helper
    {
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
    }
}
