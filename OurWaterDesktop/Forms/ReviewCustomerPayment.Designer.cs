namespace OurWaterDesktop.Forms
{
    partial class ReviewCustomerPayment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.date = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.debit = new System.Windows.Forms.Label();
            this.deadline = new System.Windows.Forms.Label();
            this.amount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fines = new System.Windows.Forms.ListBox();
            this.invoiceImg = new System.Windows.Forms.PictureBox();
            this.rejectionReasonLabel = new System.Windows.Forms.Label();
            this.rejectionReason = new System.Windows.Forms.TextBox();
            this.approve = new System.Windows.Forms.Button();
            this.reject = new System.Windows.Forms.Button();
            this.totalAmount = new System.Windows.Forms.Label();
            this.fineAmount = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceImg)).BeginInit();
            this.SuspendLayout();
            // 
            // date
            // 
            this.date.AutoSize = true;
            this.date.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date.Location = new System.Drawing.Point(23, 20);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(124, 25);
            this.date.TabIndex = 0;
            this.date.Text = "2026-01-01";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(25, 68);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(98, 16);
            this.status.TabIndex = 1;
            this.status.Text = "Status : {status}";
            // 
            // debit
            // 
            this.debit.AutoSize = true;
            this.debit.Location = new System.Drawing.Point(25, 93);
            this.debit.Name = "debit";
            this.debit.Size = new System.Drawing.Size(73, 16);
            this.debit.TabIndex = 2;
            this.debit.Text = "Debit : 0 M³";
            // 
            // deadline
            // 
            this.deadline.AutoSize = true;
            this.deadline.Location = new System.Drawing.Point(25, 120);
            this.deadline.Name = "deadline";
            this.deadline.Size = new System.Drawing.Size(135, 16);
            this.deadline.TabIndex = 3;
            this.deadline.Text = "Deadline : 2026-01-15";
            // 
            // amount
            // 
            this.amount.AutoSize = true;
            this.amount.Location = new System.Drawing.Point(25, 149);
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(107, 16);
            this.amount.TabIndex = 4;
            this.amount.Text = "Bill Amount : Rp0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fines);
            this.groupBox1.Location = new System.Drawing.Point(28, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 111);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fines";
            // 
            // fines
            // 
            this.fines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fines.FormattingEnabled = true;
            this.fines.ItemHeight = 16;
            this.fines.Location = new System.Drawing.Point(3, 18);
            this.fines.Name = "fines";
            this.fines.Size = new System.Drawing.Size(233, 90);
            this.fines.TabIndex = 0;
            // 
            // invoiceImg
            // 
            this.invoiceImg.BackColor = System.Drawing.SystemColors.ControlLight;
            this.invoiceImg.Location = new System.Drawing.Point(299, 12);
            this.invoiceImg.Name = "invoiceImg";
            this.invoiceImg.Size = new System.Drawing.Size(617, 608);
            this.invoiceImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.invoiceImg.TabIndex = 6;
            this.invoiceImg.TabStop = false;
            // 
            // rejectionReasonLabel
            // 
            this.rejectionReasonLabel.AutoSize = true;
            this.rejectionReasonLabel.Location = new System.Drawing.Point(28, 384);
            this.rejectionReasonLabel.Name = "rejectionReasonLabel";
            this.rejectionReasonLabel.Size = new System.Drawing.Size(115, 16);
            this.rejectionReasonLabel.TabIndex = 7;
            this.rejectionReasonLabel.Text = "Rejection Reason";
            // 
            // rejectionReason
            // 
            this.rejectionReason.Location = new System.Drawing.Point(25, 409);
            this.rejectionReason.Multiline = true;
            this.rejectionReason.Name = "rejectionReason";
            this.rejectionReason.Size = new System.Drawing.Size(239, 78);
            this.rejectionReason.TabIndex = 8;
            // 
            // approve
            // 
            this.approve.Location = new System.Drawing.Point(189, 493);
            this.approve.Name = "approve";
            this.approve.Size = new System.Drawing.Size(75, 30);
            this.approve.TabIndex = 9;
            this.approve.Text = "Approve";
            this.approve.UseVisualStyleBackColor = true;
            this.approve.Click += new System.EventHandler(this.onVerify);
            // 
            // reject
            // 
            this.reject.Location = new System.Drawing.Point(28, 494);
            this.reject.Name = "reject";
            this.reject.Size = new System.Drawing.Size(75, 29);
            this.reject.TabIndex = 10;
            this.reject.Text = "Reject";
            this.reject.UseVisualStyleBackColor = true;
            this.reject.Click += new System.EventHandler(this.onReject);
            // 
            // totalAmount
            // 
            this.totalAmount.AutoSize = true;
            this.totalAmount.Location = new System.Drawing.Point(25, 338);
            this.totalAmount.Name = "totalAmount";
            this.totalAmount.Size = new System.Drawing.Size(120, 16);
            this.totalAmount.TabIndex = 11;
            this.totalAmount.Text = "Total Amount : Rp0";
            // 
            // fineAmount
            // 
            this.fineAmount.AutoSize = true;
            this.fineAmount.Location = new System.Drawing.Point(25, 309);
            this.fineAmount.Name = "fineAmount";
            this.fineAmount.Size = new System.Drawing.Size(122, 16);
            this.fineAmount.TabIndex = 12;
            this.fineAmount.Text = "Fines Amount : Rp0";
            // 
            // ReviewCustomerPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 632);
            this.Controls.Add(this.fineAmount);
            this.Controls.Add(this.totalAmount);
            this.Controls.Add(this.reject);
            this.Controls.Add(this.approve);
            this.Controls.Add(this.rejectionReason);
            this.Controls.Add(this.rejectionReasonLabel);
            this.Controls.Add(this.invoiceImg);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.deadline);
            this.Controls.Add(this.debit);
            this.Controls.Add(this.status);
            this.Controls.Add(this.date);
            this.Name = "ReviewCustomerPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReviewCustomerPayment";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.invoiceImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label debit;
        private System.Windows.Forms.Label deadline;
        private System.Windows.Forms.Label amount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox fines;
        private System.Windows.Forms.PictureBox invoiceImg;
        private System.Windows.Forms.Label rejectionReasonLabel;
        private System.Windows.Forms.TextBox rejectionReason;
        private System.Windows.Forms.Button approve;
        private System.Windows.Forms.Button reject;
        private System.Windows.Forms.Label totalAmount;
        private System.Windows.Forms.Label fineAmount;
    }
}