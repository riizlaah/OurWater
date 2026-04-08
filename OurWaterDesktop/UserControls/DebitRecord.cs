using OurWaterDesktop.Models;
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
    public partial class DebitRecord : UserControl
    {
        private readonly ConsumptionDebitRecord record;
        public EventHandler<ConsumptionDebitRecord> recordClicked;
        public DebitRecord(ConsumptionDebitRecord rec)
        {
            record = rec;
            InitializeComponent();
            header.Text = $"{rec.date} - {rec.status}";
            debit.Text = $"Debit : {rec.debit:2f}M³";
            inputtedBy.Text = $"Inputted By : {rec.InputtingUser.fullname}";
            customerName.Text = $"Customer : {rec.Customer.fullname}";
        }

        protected override void OnClick(EventArgs e)
        {
            recordClicked?.Invoke(this, record);
        }
    }
}
