using OurWaterDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class Dashboard : Form
    {
        private readonly OurWater dbc;
        private readonly Login login;
        public Dashboard(OurWater _dbc, Login _login)
        {
            dbc = _dbc;
            login = _login;
            InitializeComponent();
        }

        private void manageUser(object sender, EventArgs e)
        {
            var window = new ManageUser(dbc, this);
            Hide();
            window.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            login.Show();
        }

        private void onSubmitProdDebit(object sender, EventArgs e)
        {
            var today = DateTime.Today.Date;
            var rec = dbc.ProductionDebitRecords.Where(p => p.date >= today && p.date < DbFunctions.AddDays(today, 1)).FirstOrDefault();
            var dialog = new SubmitProductionDebitRecord(dbc, rec);
            dialog.ShowDialog();
        }

        private void onViewConsumptionRecord(object sender, EventArgs e)
        {
            var window = new ViewConsumptionDebitRecords(dbc, this);
            Hide();
            window.Show();
        }

        private void onViewProdRecords(object sender, EventArgs e)
        {
            var window = new ViewProductionDebitRecords(dbc, this);
            Hide();
            window.Show();
        }

        private void onViewCustomerBills(object sender, EventArgs e)
        {
            var window = new ViewCustomerBills(dbc, this);
            Hide();
            window.Show();
        }

        private void onSettingFines(object sender, EventArgs e)
        {
            var window = new SettingFines(dbc, this);
            Hide();
            window.Show();
        }
    }
}
