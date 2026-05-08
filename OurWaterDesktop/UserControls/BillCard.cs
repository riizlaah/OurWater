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

namespace OurWaterDesktop.UserControls
{
    public partial class BillCard : UserControl
    {
        private readonly BillObj record;
        public EventHandler<BillObj> CardClick { get; set; }
        public BillCard(BillObj rec)
        {
            record = rec;
            InitializeComponent();
            headerLb.Text = $"{rec.createdAt:dd-MM-yyyy} - ({rec.status})";
            customerName.Text = $"Customer Name : {rec.customer.name}";
            debitLb.Text = $"Debit : {rec.consumptionDebitRecord.debit:F2}";
            totalAmount.Text = $"Submitted By : {rec.totalAmount}";
            deadline.Text = $"Location : {rec.deadline:dd MMMM yyyy}";
        }

        protected override void OnClick(EventArgs e)
        {
            CardClick?.Invoke(this, record);
        }
    }


}
