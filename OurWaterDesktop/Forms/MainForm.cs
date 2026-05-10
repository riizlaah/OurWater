using OurWaterDesktop.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Views
{
    public partial class MainForm : Form
    {
        private readonly System.Windows.Forms.Timer timer;
        private readonly Login loginForm;
        public MainForm(Login login)
        {
            loginForm = login;
            timer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            timer.Tick += (s, e) =>
            {
                dateTimeLb.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy (HH:mm:ss)");
            };
            InitializeComponent();
            greetLb.Text = "Hello " + Helper.CurrentSession?.fullname ?? "Admin";
            timer.Start();
            dateTimeLb.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy (HH:mm:ss)");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No;
        }

        protected override void OnClosed(EventArgs e)
        {
            loginForm.Show();
        }

        public void OnViewConsDebitRec(object s, EventArgs e)
        {
            var window = new ViewConsumptionDebitRecordsForm(this);
            Hide();
            window.Show();
        }

        private void OnViewCustomerBills(object sender, EventArgs e)
        {
            var window = new ViewCustomerBillsForm(this);
            Hide();
            window.Show();
        }

        private void OnViewProdDebitRecs(object sender, EventArgs e)
        {
            var window = new ViewProductionDebitRecordsForm(this);
            Hide();
            window.Show();
        }

        private void OnSubmitProdDebitRec(object sender, EventArgs e)
        {
            var dialog = new SubmitProdDebit(null);
            dialog.ShowDialog();
        }

        private void OnManageUsers(object sender, EventArgs e)
        {
            var window = new ManageUsersForm(this);
            Hide();
            window.Show();
        }

        private void OnSettingFineRules(object sender, EventArgs e)
        {
            var window = new ManageFinesForm(this);
            Hide();
            window.Show();
        }

        private void OnViewWaterUsage(object sender, EventArgs e)
        {
            var window = new ViewWaterUsageForm(this);
            Hide();
            window.Show();
        }
    }
}
