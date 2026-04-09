using OurWaterDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class SubmitProductionDebitRecord : Form
    {
        private readonly OurWater dbc;
        private readonly Dashboard dash;
        private readonly ProductionDebitRecord record = null;

        public SubmitProductionDebitRecord(OurWater ctx, ProductionDebitRecord rec = null)
        {
            dbc = ctx;
            record = rec;
            InitializeComponent();
            if (rec != null)
            {
                date.Text = rec.date.ToString("yyyy-MM-dd");
                debit.Text = rec.debit.ToString();
            } else
            {
                date.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        private void onSave(object sender, EventArgs e)
        {
            if(!decimal.TryParse(debit.Text, out decimal debitDecimal))
            {
                MessageBox.Show("Debit not valid");
                return;
            }
            if(debitDecimal < 0.00001m)
            {
                MessageBox.Show("Debit not valid");
                return;
            }
            if (record == null)
            {
                dbc.ProductionDebitRecords.Add(new ProductionDebitRecord {
                    date = DateTime.Today,
                    debit = debitDecimal,
                    inputtedBy = dbc.User.id,
                    location = dbc.User.address
                });
                dbc.SaveChanges();
            } else
            {
                var rec = dbc.ProductionDebitRecords.Find(record.id);
                rec.debit = debitDecimal;
                dbc.SaveChanges();
            }
            Close();
        }

        private void onCancel(object sender, EventArgs e)
        {
            Close();
        }
    }
}
