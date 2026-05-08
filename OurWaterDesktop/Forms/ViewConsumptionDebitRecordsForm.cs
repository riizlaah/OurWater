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
    public partial class ViewConsumptionDebitRecordsForm : Form
    {
        private readonly MainForm main;
        public ViewConsumptionDebitRecordsForm(MainForm m)
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
            var (isSuccess, result) = await Helper.JsonReq<object, List<ConsDebitRecord>>("ConsumptionDebits");
            if (!isSuccess || result.data == null) return;
            foreach (var item in result.data)
            {
                var card = new ConsumptionDebitCard(item);
                flowLayoutPanel1.Controls.Add(card);
                card.CardClick += (s, rec) =>
                {
                    var window = new ReviewConsumptionDebitRecordForm(this, rec.id);
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
}
