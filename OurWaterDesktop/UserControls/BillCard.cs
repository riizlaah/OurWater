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
    public partial class BillCard : UserControl
    {
        private readonly Bill bill;
        public EventHandler<Bill> BillClicked;
        public BillCard(Bill _bill)
        {
            bill = _bill;
            InitializeComponent();
            customerName.Text = $"Customer : {bill.Customer.fullname}";
            header.Text = $"{bill.createdAt:yyyy-MM-dd} - {bill.status}";
            debit.Text = $"Debit : {bill.ConsumptionDebitRecord.debit} M³";
            amount.Text = $"Total Amount : {bill.calculateTotal():Rp#,##0;(Rp#,##0);Rp0}";
            debit.Text = $"Deadline : {bill.deadline:yyyy-MM-dd}";
        }

        protected override void OnClick(EventArgs e)
        {
            BillClicked?.Invoke(this, bill);
        }
        
    }
}
