using OurWaterDesktop.Models;
using OurWaterDesktop.UserControls;
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
    public partial class ViewConsumptionDebitRecords : Form
    {
        private readonly OurWater dbc;
        private readonly Dashboard dash;
        public ViewConsumptionDebitRecords(OurWater ctx, Dashboard dashb)
        {
            dbc = ctx;
            dash = dashb;
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            flowLayoutPanel1.Controls.Clear();
            var items = dbc.ConsumptionDebitRecords.Include("Customer").Include("InputtingUser").OrderByDescending(c => c.date).ToList();
            foreach(var rec in items)
            {
                var item = new DebitRecord(rec);
                flowLayoutPanel1.Controls.Add(item);
                item.recordClicked += OnClicked;
            }
        }

        private void OnClicked(object sender, ConsumptionDebitRecord rec)
        {
            var reviewDialog = new ReviewConsumptionDebitRecord(rec, dbc);
            reviewDialog.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            dash.Show();
        }
    }
}
