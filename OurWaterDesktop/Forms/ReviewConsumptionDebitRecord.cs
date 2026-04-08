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
using System.Net.Http;

namespace OurWaterDesktop.Forms
{
    public partial class ReviewConsumptionDebitRecord : Form
    {
        private readonly ConsumptionDebitRecord record;
        private readonly OurWater dbc;
        public ReviewConsumptionDebitRecord(ConsumptionDebitRecord rec, OurWater ctx)
        {
            dbc = ctx;
            record = rec;
            InitializeComponent();
            date.Text = rec.date.ToString();
            status.Text = "Status : " + rec.status.ToString();
            debit.Text = $"Debit : {rec.debit:2f}M³";
            customerName.Text = $"Name : {rec.Customer.fullname}";
            inputtedByName.Text = $"Name : {rec.InputtingUser.fullname}";
            if(rec.InputtingUser.role == "officer")
            {
                inputtedByType.Text += "Officer";
            } else
            {
                inputtedByType.Text += "Customer";
            }
            if(rec.correctedBy == null)
            {
                correctedByGroup.Hide();
            } else
            {
                correctedByName.Text = $"Name : {rec.CorrectingUser.fullname}";
            }
            if(rec.rejectionReason != "")
            {
                rejectionReason.Text = rec.rejectionReason;
            }
            if(rec.status == "Verified")
            {
                rejectionReason.Hide();
                reject.Hide();
                verify.Hide();
            }
        }

        private void onVerify(object sender, EventArgs e)
        {
            var rec = dbc.ConsumptionDebitRecords.Find(record.id);
            rec.rejectionReason = "";
            rec.status = "Verified";
            dbc.SaveChanges();
            Close();
        }

        private void onReject(object sender, EventArgs e)
        {
            if(rejectionReason.Text.Trim() == "")
            {
                MessageBox.Show("Rejection Reason required");
                return;
            }
            var rec = dbc.ConsumptionDebitRecords.Find(record.id);
            rec.rejectionReason = rejectionReason.Text;
            rec.status = "Rejected";
            dbc.SaveChanges();
            Close();
        }
    }
}
