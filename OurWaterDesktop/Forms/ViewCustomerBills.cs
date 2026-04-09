using OurWaterDesktop.Models;
using OurWaterDesktop.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class ViewCustomerBills : Form
    {
        private readonly OurWater dbc;
        private readonly Dashboard dash;
        public ViewCustomerBills(OurWater ctx, Dashboard dashb)
        {
            dbc = ctx;
            dash = dashb;
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            var items = dbc.Bills.ToList();
            var fineRules = dbc.FineRules.ToList();
            items = items.Where(it => it.status != "Approved").ToList();
            var hasChanged = false;
            foreach(var it in items) {
                foreach (var rule in fineRules)
                {
                    if(DateTime.Now.Subtract(it.deadline).TotalDays >= rule.dayAfterDeadline)
                    {
                        if (dbc.Fines.Any(f => f.fineRuleId == rule.id && f.billId == it.id)) continue;
                        dbc.Fines.Add(new Fine { billId = it.id, fineRuleId = rule.id, createdAt = DateTime.Now });
                        hasChanged = true;
                    }
                }
            }
            if(hasChanged) dbc.SaveChanges();
            items = dbc.Bills.AsNoTracking().Include("Customer").Include("ConsumptionDebitRecord").Include("Fines.FineRule").ToList();
            flowLayoutPanel1.Controls.Clear();
            foreach(var bill in items)
            {
                var card = new BillCard(bill);
                flowLayoutPanel1.Controls.Add(card);
                card.BillClicked += OnBillCardClicked;
            }
        }

        private void OnBillCardClicked(object sender, Bill bill)
        {
            var dialog = new ReviewCustomerPayment(dbc, bill);
            dialog.ShowDialog();
            RefreshData();
        }

        protected override void OnClosed(EventArgs e)
        {
            dash.Show();
        }
    }
}
