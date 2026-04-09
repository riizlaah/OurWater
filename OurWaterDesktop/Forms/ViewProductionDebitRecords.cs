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
    public partial class ViewProductionDebitRecords : Form
    {
        private readonly OurWater dbc;
        private readonly Dashboard dash;
        public ViewProductionDebitRecords(OurWater ctx, Dashboard dashb)
        {
            dbc = ctx;
            dash = dashb;
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            flowLayoutPanel1.Controls.Clear();
            var items = dbc.ProductionDebitRecords.Include("InputtingUser").OrderByDescending(c => c.date).ToList();
            foreach(var rec in items)
            {
                var item = new DebitRecord(null, rec);
                flowLayoutPanel1.Controls.Add(item);
                item.prodRecordClicked += OnClicked;
            }
        }

        private void OnClicked(object sender, ProductionDebitRecord rec)
        {
            var reviewDialog = new SubmitProductionDebitRecord(dbc, rec);
            reviewDialog.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            dash.Show();
        }
    }
}
