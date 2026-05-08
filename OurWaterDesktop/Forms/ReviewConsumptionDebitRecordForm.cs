using OurWaterDesktop.UserControls;
using OurWaterDesktop.Views;
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
    public partial class ReviewConsumptionDebitRecordForm : Form
    {
        private readonly ViewConsumptionDebitRecordsForm parent;
        private readonly int id;
        public ReviewConsumptionDebitRecordForm(ViewConsumptionDebitRecordsForm p, int id)
        {
            parent = p;
            InitializeComponent();
            this.id = id;
            FetchData();
        }

        protected override void OnClosed(EventArgs e)
        {
            parent.Show();
        }

        async private void FetchData()
        {
            var (isSuccess, result) = await Helper.JsonReq<object, DetailedConsDebitRec>($"ConsumptionDebits/{id}");
            if (result.data == null || !isSuccess)
            {
                Close();
                return;
            }
            ;
            var rec = result.data;
            headerLb.Text = $"{rec.date} - ({rec.status})";
            customerName.Text = $"Customer Name : {rec.customerName}";
            submittedBy.Text = $"Submitted By : {rec.inputtedBy}";
            if (rec.correctedBy != null)
            {
                correctedBy.Text = $"Corrected By : {rec.correctedBy}";
            }
            else
            {
                correctedBy.Hide();
            }
            debitLb.Text = $"Debit : {rec.debit:F2}";
            previousDebit.Text = "Previous Debit : " + (rec.prevDebit.HasValue ? rec.prevDebit.Value.ToString("F2") : "?");
            rejectionReason.Text = rec.rejectionReason;
            if (rec.status != "Pending")
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
            var proofImg = await Helper.FetchImg(rec.imagePath);
            if (proofImg != null)
            {
                image.Image = proofImg;
            }

        }

        private void OnReject(object sender, EventArgs e)
        {
            if (rejectionReason.Text.Trim() == "")
            {
                MessageBox.Show("Rejection reason required");
                return;
            }
            UpdateStatus("Rejected", rejectionReason.Text.Trim());
        }

        private void OnVerify(object sender, EventArgs e)
        {
            UpdateStatus("Verified");
        }

        async private Task UpdateStatus(string status, string rejection = "")
        {
            var (isSuccess, result) = await Helper.JsonReq<PatchConsDebitRec, object>($"ConsumptionDebits/{id}", new PatchConsDebitRec
            {
                rejectionReason = rejection,
                status = status
            }, "patch");
            if(isSuccess)
            {
                FetchData();
            } else
            {
                MessageBox.Show(result.message, "Error");
            }
        }
    }


    public class PatchConsDebitRec
    {
        public string rejectionReason { get; set; }
        public string status { get; set; }
    }


    public class DetailedConsDebitRec
    {
        public int id { get; set; }
        public string customerName { get; set; }
        public string inputtedBy { get; set; }
        public string correctedBy { get; set; }
        public double debit { get; set; }
        public double? prevDebit { get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public string location { get; set; }
        public DateTime updatedAt { get; set; }
        public string imagePath { get; set; }
        public string rejectionReason { get; set; }
    }


}
