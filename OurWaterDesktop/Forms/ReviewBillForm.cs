using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class ReviewBillForm : Form
    {
        private readonly ViewCustomerBillsForm parent;
        private readonly int id;
        public ReviewBillForm(ViewCustomerBillsForm p, int id)
        {
            this.id = id;
            parent = p;
            InitializeComponent();
            FetchData();

        }

        async private void FetchData()
        {
            var (isSuccess, result) = await Helper.JsonReq<object, DetailedBillObj>($"Bills/{id}");
            if (result.data == null || !isSuccess)
            {
                Close();
                return;
            }
            ;
            var rec = result.data;
            headerLb.Text = $"{rec.createdAt:dd-MM-yyyy} - ({rec.status})";
            deadline.Text = $"Deadline : {rec.deadline:dd-MM-yyyy}";
            customerName.Text = $"Customer Name : {rec.customer.name}";
            customerAddress.Text = $"Address : {rec.customer.address}";
            debitLb.Text = $"Debit : {rec.consumptionDebitRecord.debit:F2}";
            rejectionReason.Text = rec.rejectionReason;
            originalAmount.Text = $"Original Amount : {rec.originalAmount:Rp#,##0;(Rp#,##0);Rp0}";
            fineAmount.Text = $"Fine Amount : {rec.extraFine:Rp#,##0;(Rp#,##0);Rp0}";
            if (rec.fines.Length > 0) fineDetails.DataSource = rec.fines;
            else fineDetails.Hide();
            totalAmount.Text = $"Total Amount : {rec.totalAmount:Rp#,##0;(Rp#,##0);Rp0}";
            if (rec.status != "Paid Unconfirmed")
            {
                reject.Hide();
                verify.Hide();
                rejectionReason.ReadOnly = true;
            }
            else
            {
                reject.Show();
                verify.Show();
                rejectionReason.ReadOnly = false;
            }
            if (rec.imagePath == null) return;
            var proofImg = await Helper.FetchImg(rec.imagePath);
            if (proofImg != null)
            {
                image.Image = proofImg;
            }

        }

        protected override void OnClosed(EventArgs e)
        {
            parent.Show();
        }

        private void onVerify(object sender, EventArgs e)
        {
            UpdateStatus("Paid");
        }

        private void onReject(object sender, EventArgs e)
        {
            if(rejectionReason.Text.Trim() == "")
            {
                MessageBox.Show("Rejection reason required");
                return;
            }
            UpdateStatus("Rejected", rejectionReason.Text.Trim());
        }

        async private Task UpdateStatus(string status, string rejection = "")
        {
            var (isSuccess, result) = await Helper.JsonReq<PatchConsDebitRec, object>($"Bills/{id}", new PatchConsDebitRec
            {
                rejectionReason = rejection,
                status = status
            }, "patch");
            if (isSuccess)
            {
                FetchData();
            }
            else
            {
                MessageBox.Show(result.message, "Error");
            }
        }
    }


    public class DetailedBillObj
    {
        public int id { get; set; }
        public ConsDebitObj consumptionDebitRecord { get; set; }
        public DetailedCustomer customer { get; set; }
        public decimal originalAmount { get; set; }
        public decimal extraFine { get; set; }
        public string[] fines { get; set; }
        public decimal totalAmount { get; set; }
        public DateTime deadline { get; set; }
        public string status { get; set; }
        public string rejectionReason { get; set; }
        public string? imagePath { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class DetailedCustomer
    {
        public string name { get; set; }
        public string address { get; set; }
    }

}
