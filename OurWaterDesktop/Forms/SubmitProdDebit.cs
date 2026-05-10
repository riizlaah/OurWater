using OurWaterDesktop.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class SubmitProdDebit : Form
    {
        private ProdDebit? prodDebit = null;
        public SubmitProdDebit(ProdDebit? p)
        {
            if (p != null) prodDebit = p;
            //else prodDebit = new ProdDebit();
            InitializeComponent();
            if (prodDebit != null)
            {
                datePicker.Value = prodDebit.date.ToDateTime(new TimeOnly(0, 0, 0));
                datePicker.Enabled = false;
                debit.Text = prodDebit.debit.ToString("F2");
            } else
            {
                datePicker.Value = DateTime.Now;
            }
        }

        private void onSave(object sender, EventArgs e)
        {
            if(!decimal.TryParse(debit.Text, out decimal actualDebit))
            {
                MessageBox.Show("Debit not valid");
                return;
            }
            SendReq(actualDebit);
        }

        async private void SendReq(decimal actualDebit)
        {
            var url = "ProductionDebits";
            var method = prodDebit == null ? "post" : "put";
            if (prodDebit != null) url += $"/{prodDebit.id}";
            var (success, result) = await Helper.JsonReq<ProdDebitReq, object>(url, new ProdDebitReq { date = DateOnly.FromDateTime(datePicker.Value), debit = actualDebit }, method);
            //Debug.WriteLine(url, method);
            if(!success)
            {
                MessageBox.Show(result.message, "Error");
            }
            Close();
        }

        private void onCancel(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class ProdDebitReq
    {
        public DateOnly date { get; set; }
        public decimal debit { get; set; }
    }
}
