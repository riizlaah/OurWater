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
    public partial class ProdDebitCard : UserControl
    {
        private readonly ProdDebit record;
        public EventHandler<ProdDebit> CardClick;
        public ProdDebitCard(ProdDebit record)
        {
            this.record = record;
            InitializeComponent();
            day.Text = record.date.ToString("dd");
            debitLb.Text = $"Debit : {record.debit:F2}";
        }

        protected override void OnClick(EventArgs e)
        {
            CardClick?.Invoke(this, record);
        }

    }

    public class ProdDebit
    {
        public int id { get; set; }
        public decimal debit { get; set; }
        public DateOnly date { get; set; }
    }
}
