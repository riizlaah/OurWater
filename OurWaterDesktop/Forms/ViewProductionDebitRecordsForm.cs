using OurWaterDesktop.Forms;
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

namespace OurWaterDesktop.Views
{
    public partial class ViewProductionDebitRecordsForm : Form
    {
        private readonly MainForm main;
        private bool initialized = false;
        public ViewProductionDebitRecordsForm(MainForm m)
        {
            main = m;
            InitializeComponent();
            var months = new List<Month>();
            var currYear = DateTime.Today.Year;
            var selectedIdx = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (i == DateTime.Today.Month) selectedIdx = i - 1;
                months.Add(new Month { Name = new DateTime(currYear, i, 1).ToString("MMMM"), Number = i });
            }
            monthPicker.DisplayMember = "Name";
            monthPicker.DataSource = months;
            monthPicker.SelectedIndex = selectedIdx;
            yearInp.Maximum = decimal.MaxValue;
            yearInp.Value = currYear;
            initialized = true;
            RefreshData();
        }

        protected override void OnClosed(EventArgs e)
        {
            main.Show();
        }

        async private void RefreshData()
        {
            if (!initialized) return;
            flowLayoutPanel1.Controls.Clear();
            var (isSuccess, result) = await Helper.JsonReq<object, List<ProdDebit>>($"ProductionDebits?month={(monthPicker.SelectedItem as Month)?.Number ?? DateTime.Today.Month}&year={(yearInp.Value == 0m ? DateTime.Now.Year : yearInp.Value)}");
            if (!isSuccess || result.data == null) return;
            foreach (var item in result.data)
            {
                var card = new ProdDebitCard(item);
                flowLayoutPanel1.Controls.Add(card);
                card.CardClick += (s, rec) =>
                {
                    var window = new SubmitProdDebit(rec);
                    window.ShowDialog();
                    RefreshData();
                };
            }
        }

        private void OnYearChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void OnMonthChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }

    public class Month
    {
        public string Name { get; set; } = null!;

        public int Number { get; set; }
    }
}
