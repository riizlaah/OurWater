using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.UserControls
{
    public partial class ConsumptionDebitCard : UserControl
    {
        private readonly ConsDebitRecord record;
        public EventHandler<ConsDebitRecord> CardClick { get; set; }
        public ConsumptionDebitCard(ConsDebitRecord rec)
        {
            record = rec;
            InitializeComponent();
            headerLb.Text = $"{rec.date} - ({rec.status})";
            customerName.Text = $"Customer Name : {rec.customerName}";
            debitLb.Text = $"Debit : {rec.debit:F2}";
            submittedBy.Text = $"Submitted By : {rec.inputtedBy}";
            location.Text = $"Location : {rec.location}";
        }

        protected override void OnClick(EventArgs e)
        {
            CardClick?.Invoke(this, record);
        }
    }


    public class ConsDebitRecord
    {
        public int id { get; set; }
        public string customerName { get; set; }
        public string inputtedBy { get; set; }
        public string correctedBy { get; set; }
        public float debit { get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public string location { get; set; }
        public DateTime updatedAt { get; set; }
    }

}
