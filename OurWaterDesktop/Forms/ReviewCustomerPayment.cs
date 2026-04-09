using OurWaterDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class ReviewCustomerPayment : Form
    {
        private readonly HttpClient _http;
        private readonly OurWater dbc;
        private readonly Bill bill;

        public ReviewCustomerPayment(OurWater ctx, Bill _bill)
        {
            _http = new HttpClient();
            dbc = ctx;
            bill = _bill;
            InitializeComponent();
            date.Text = bill.createdAt.ToString("yyyy-MM-dd");
            status.Text = $"Status : {bill.status}";
            debit.Text = $"Debit : {bill.ConsumptionDebitRecord.debit} M³";
            amount.Text = $"Bill Amount : {bill.amount:Rp#,##0;(Rp#,##0);Rp0}";
            deadline.Text = $"Deadline {bill.deadline:yyyy-MM-dd}";
            fines.DataSource = bill.Fines.ToList();
            fines.DisplayMember = "FineRuleStr";
            fineAmount.Text = $"Fines Amount : {bill.calculateFines():Rp#,##0;(Rp#,##0);Rp0}";
            totalAmount.Text = $"Total Amount : {bill.calculateTotal():Rp#,##0;(Rp#,##0);Rp0}";
            if(bill.status == "Rejected" || bill.status == "Approved")
            {
                approve.Hide();
                reject.Hide();
                rejectionReason.ReadOnly = true;
            }
            if(bill.rejectionReason != "")
            {
                rejectionReason.Text = bill.rejectionReason;
            }
            if(bill.imagePath != null) loadImg(bill.imagePath);
        }
        private async Task loadImg(string path)
        {
            invoiceImg.Image = await Helper.loadImage(path, _http);
        }

        private void onReject(object sender, EventArgs e)
        {
            if(rejectionReason.Text.Trim().Length == 0)
            {
                MessageBox.Show("Rejection reason required");
                return;
            }
            var bill2 = dbc.Bills.Find(bill.id);
            bill2.status = "Rejected";
            bill2.rejectionReason = rejectionReason.Text;
            bill2.updatedAt = DateTime.Now;
            dbc.SaveChanges();
            Close();
        }

        private void onVerify(object sender, EventArgs e)
        {
            if (invoiceImg.Image == null) return;
            var bill2 = dbc.Bills.Find(bill.id);
            bill2.status = "Approved";
            bill2.rejectionReason = "";
            bill2.updatedAt = DateTime.Now;
            dbc.SaveChanges();
            Close();
        }
    }
}
