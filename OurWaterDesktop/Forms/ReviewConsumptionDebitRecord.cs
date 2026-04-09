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
        private readonly HttpClient _http;
        private readonly ConsumptionDebitRecord record;
        private readonly OurWater dbc;
        
        public ReviewConsumptionDebitRecord(ConsumptionDebitRecord rec, OurWater ctx)
        {
            _http = new HttpClient();
            dbc = ctx;
            record = rec;
            InitializeComponent();
            date.Text = rec.date.ToString("yyyy-MM-dd");
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
            loadImg(rec.imagePath);
        }

        private async Task loadImg(string path)
        {
            pictureBox1.Image = await Helper.loadImage(path, _http);
        }

        private void onVerify(object sender, EventArgs e)
        {
            var rec = dbc.ConsumptionDebitRecords.Find(record.id);
            rec.rejectionReason = "";
            rec.status = "Verified";
            rec.updatedAt = DateTime.Now;
            var amount = 0m;
            if(rec.debit < 10m)
            {
                amount = rec.debit * 2500m;
            } else if(rec.debit < 20m)
            {
                amount = (10 * 2500m) + ((rec.debit - 10m) * 3500m);
            } else
            {
                amount = (10 * 2500m) + (10m * 3500m) + ((rec.debit - 20m) * 5000);
            }
            dbc.Bills.Add(new Bill
            {
                customerId = rec.customerId,
                consumptionRecordId = rec.id,
                amount = amount,
                deadline = DateTime.Now.AddDays(14),
                status = "Pending",
                updatedAt = DateTime.Now,
                createdAt = DateTime.Now,
                rejectionReason = "",
                imagePath = null
            });
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
            rec.updatedAt = DateTime.Now;
            dbc.SaveChanges();
            Close();
        }
    }
}
