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
        private readonly ProductionDebitRecord prodRecord;
        public EventHandler<ConsumptionDebitRecord> recordClicked;
        public EventHandler<ProductionDebitRecord> prodRecordClicked;
        public DebitRecord(ConsumptionDebitRecord rec, ProductionDebitRecord prodRec)
        {
            record = rec;
            prodRecord = prodRec;
            InitializeComponent();
            if(rec != null)
            {
                header.Text = $"{rec.date:yyyy-MM-dd} - {rec.status}";
                debit.Text = $"Debit : {rec.debit} M³";
                inputtedBy.Text = $"Inputted By : {rec.InputtingUser.fullname}";
                customerName.Text = $"Customer : {rec.Customer.fullname}";
            } else
            {
                header.Text = $"{prodRec.date:yyyy-MM-dd}";
                debit.Text = $"Debit : {prodRec.debit} M³";
                inputtedBy.Text = $"Inputted By : {prodRec.InputtingUser.fullname}";
                customerName.Hide();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            recordClicked?.Invoke(this, record);
            prodRecordClicked?.Invoke(this, prodRecord);
        }
    }
}
