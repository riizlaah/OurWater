namespace OurWaterDesktop.Forms
{
    partial class ReviewBillForm
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
            totalAmount = new Label();
            reject = new Button();
            verify = new Button();
            rejectionReason = new TextBox();
            image = new PictureBox();
            fineAmount = new Label();
            originalAmount = new Label();
            customerAddress = new Label();
            customerName = new Label();
            headerLb = new Label();
            fineDetails = new ListBox();
            deadline = new Label();
            debitLb = new Label();
            ((System.ComponentModel.ISupportInitialize)image).BeginInit();
            SuspendLayout();
            // 
            // totalAmount
            // 
            totalAmount.AutoSize = true;
            totalAmount.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            totalAmount.ForeColor = SystemColors.HotTrack;
            totalAmount.Location = new Point(11, 414);
            totalAmount.Name = "totalAmount";
            totalAmount.Size = new Size(157, 25);
            totalAmount.TabIndex = 19;
            totalAmount.Text = "Total Amount : {}";
            // 
            // reject
            // 
            reject.Location = new Point(243, 561);
            reject.Name = "reject";
            reject.Size = new Size(94, 29);
            reject.TabIndex = 18;
            reject.Text = "Reject";
            reject.UseVisualStyleBackColor = true;
            reject.Click += onReject;
            // 
            // verify
            // 
            verify.Location = new Point(11, 561);
            verify.Name = "verify";
            verify.Size = new Size(94, 29);
            verify.TabIndex = 17;
            verify.Text = "Verify";
            verify.UseVisualStyleBackColor = true;
            verify.Click += onVerify;
            // 
            // rejectionReason
            // 
            rejectionReason.Location = new Point(11, 471);
            rejectionReason.Multiline = true;
            rejectionReason.Name = "rejectionReason";
            rejectionReason.Size = new Size(326, 68);
            rejectionReason.TabIndex = 16;
            // 
            // image
            // 
            image.BackColor = SystemColors.ControlLight;
            image.Location = new Point(389, 12);
            image.Name = "image";
            image.Size = new Size(535, 578);
            image.SizeMode = PictureBoxSizeMode.Zoom;
            image.TabIndex = 15;
            image.TabStop = false;
            // 
            // fineAmount
            // 
            fineAmount.AutoSize = true;
            fineAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            fineAmount.Location = new Point(11, 252);
            fineAmount.Name = "fineAmount";
            fineAmount.Size = new Size(124, 20);
            fineAmount.TabIndex = 14;
            fineAmount.Text = "Fine Amount : {}";
            // 
            // originalAmount
            // 
            originalAmount.AutoSize = true;
            originalAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            originalAmount.Location = new Point(11, 223);
            originalAmount.Name = "originalAmount";
            originalAmount.Size = new Size(150, 20);
            originalAmount.TabIndex = 13;
            originalAmount.Text = "Original Amount : {}";
            // 
            // customerAddress
            // 
            customerAddress.AutoEllipsis = true;
            customerAddress.Location = new Point(12, 119);
            customerAddress.Name = "customerAddress";
            customerAddress.Size = new Size(327, 40);
            customerAddress.TabIndex = 12;
            customerAddress.Text = "Address : {}";
            // 
            // customerName
            // 
            customerName.AutoSize = true;
            customerName.Location = new Point(12, 88);
            customerName.Name = "customerName";
            customerName.Size = new Size(137, 20);
            customerName.TabIndex = 11;
            customerName.Text = "Customer Name : {}";
            // 
            // headerLb
            // 
            headerLb.AutoSize = true;
            headerLb.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            headerLb.Location = new Point(11, 9);
            headerLb.Name = "headerLb";
            headerLb.Size = new Size(194, 31);
            headerLb.TabIndex = 10;
            headerLb.Text = "{date} - ({status})";
            // 
            // fineDetails
            // 
            fineDetails.FormattingEnabled = true;
            fineDetails.Location = new Point(11, 279);
            fineDetails.Name = "fineDetails";
            fineDetails.Size = new Size(326, 124);
            fineDetails.TabIndex = 20;
            // 
            // deadline
            // 
            deadline.AutoSize = true;
            deadline.Location = new Point(12, 55);
            deadline.Name = "deadline";
            deadline.Size = new Size(90, 20);
            deadline.TabIndex = 21;
            deadline.Text = "Deadline : {}";
            // 
            // debitLb
            // 
            debitLb.AutoSize = true;
            debitLb.Location = new Point(12, 176);
            debitLb.Name = "debitLb";
            debitLb.Size = new Size(67, 20);
            debitLb.TabIndex = 22;
            debitLb.Text = "Debit : {}";
            // 
            // ReviewBillForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(936, 606);
            Controls.Add(debitLb);
            Controls.Add(deadline);
            Controls.Add(fineDetails);
            Controls.Add(totalAmount);
            Controls.Add(reject);
            Controls.Add(verify);
            Controls.Add(rejectionReason);
            Controls.Add(image);
            Controls.Add(fineAmount);
            Controls.Add(originalAmount);
            Controls.Add(customerAddress);
            Controls.Add(customerName);
            Controls.Add(headerLb);
            Name = "ReviewBillForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ReviewBillForm";
            ((System.ComponentModel.ISupportInitialize)image).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label totalAmount;
        private Button reject;
        private Button verify;
        private TextBox rejectionReason;
        private PictureBox image;
        private Label fineAmount;
        private Label originalAmount;
        private Label customerAddress;
        private Label customerName;
        private Label headerLb;
        private ListBox fineDetails;
        private Label deadline;
        private Label debitLb;
    }
}