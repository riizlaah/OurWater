using OurWaterDesktop.UserControls;
using OurWaterDesktop.Views;
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
    public partial class ViewCustomerBillsForm : Form
    {
        private readonly MainForm main;
        public ViewCustomerBillsForm(MainForm m)
        {
            main = m;
            InitializeComponent();
            RefreshData();
        }

        protected override void OnClosed(EventArgs e)
        {
            main.Show();
        }

        async private void RefreshData()
        {
            flowLayoutPanel1.Controls.Clear();
            var (isSuccess, result) = await Helper.JsonReq<object, List<BillObj>>("Bills");
            if (!isSuccess || result.data == null) return;
            foreach (var item in result.data)
            {
                var card = new BillCard(item);
                flowLayoutPanel1.Controls.Add(card);
                card.CardClick += (s, rec) =>
                {
                    var window = new ReviewBillForm(this, rec.id);
                    Hide();
                    window.Show();
                    window.FormClosed += (s, e) =>
                    {
                        RefreshData();
                    };
                };
            }
        }
    }


    public class BillObj
    {
        public int id { get; set; }
        public ConsDebitObj consumptionDebitRecord { get; set; }
        public CustomerObj customer { get; set; }
        public decimal originalAmount { get; set; }
        public decimal extraFine { get; set; }
        public decimal totalAmount { get; set; }
        public DateTime deadline { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class ConsDebitObj
    {
        public int id { get; set; }
        public decimal debit { get; set; }
    }

    public class CustomerObj
    {
        public string name { get; set; }
    }

}
