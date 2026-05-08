namespace OurWaterDesktop.Forms
{
    partial class ReviewConsumptionDebitRecordForm
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
            headerLb = new Label();
            customerName = new Label();
            submittedBy = new Label();
            correctedBy = new Label();
            debitLb = new Label();
            image = new PictureBox();
            rejectionReason = new TextBox();
            verify = new Button();
            reject = new Button();
            previousDebit = new Label();
            ((System.ComponentModel.ISupportInitialize)image).BeginInit();
            SuspendLayout();
            // 
            // headerLb
            // 
            headerLb.AutoSize = true;
            headerLb.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            headerLb.Location = new Point(12, 9);
            headerLb.Name = "headerLb";
            headerLb.Size = new Size(194, 31);
            headerLb.TabIndex = 0;
            headerLb.Text = "{date} - ({status})";
            // 
            // customerName
            // 
            customerName.AutoSize = true;
            customerName.Location = new Point(12, 81);
            customerName.Name = "customerName";
            customerName.Size = new Size(137, 20);
            customerName.TabIndex = 1;
            customerName.Text = "Customer Name : {}";
            // 
            // submittedBy
            // 
            submittedBy.AutoSize = true;
            submittedBy.Location = new Point(12, 118);
            submittedBy.Name = "submittedBy";
            submittedBy.Size = new Size(119, 20);
            submittedBy.TabIndex = 2;
            submittedBy.Text = "Submitted By : {}";
            // 
            // correctedBy
            // 
            correctedBy.AutoSize = true;
            correctedBy.Location = new Point(12, 161);
            correctedBy.Name = "correctedBy";
            correctedBy.Size = new Size(115, 20);
            correctedBy.TabIndex = 3;
            correctedBy.Text = "Corrected By : {}";
            // 
            // debitLb
            // 
            debitLb.AutoSize = true;
            debitLb.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            debitLb.Location = new Point(15, 205);
            debitLb.Name = "debitLb";
            debitLb.Size = new Size(71, 20);
            debitLb.TabIndex = 4;
            debitLb.Text = "Debit : {}";
            // 
            // image
            // 
            image.BackColor = SystemColors.ControlLight;
            image.Location = new Point(351, 12);
            image.Name = "image";
            image.Size = new Size(404, 373);
            image.SizeMode = PictureBoxSizeMode.Zoom;
            image.TabIndex = 5;
            image.TabStop = false;
            // 
            // rejectionReason
            // 
            rejectionReason.Location = new Point(13, 270);
            rejectionReason.Multiline = true;
            rejectionReason.Name = "rejectionReason";
            rejectionReason.Size = new Size(323, 68);
            rejectionReason.TabIndex = 6;
            // 
            // verify
            // 
            verify.Location = new Point(15, 349);
            verify.Name = "verify";
            verify.Size = new Size(94, 29);
            verify.TabIndex = 7;
            verify.Text = "Verify";
            verify.UseVisualStyleBackColor = true;
            verify.Click += OnVerify;
            // 
            // reject
            // 
            reject.Location = new Point(242, 349);
            reject.Name = "reject";
            reject.Size = new Size(94, 29);
            reject.TabIndex = 8;
            reject.Text = "Reject";
            reject.UseVisualStyleBackColor = true;
            reject.Click += OnReject;
            // 
            // previousDebit
            // 
            previousDebit.AutoSize = true;
            previousDebit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            previousDebit.Location = new Point(15, 236);
            previousDebit.Name = "previousDebit";
            previousDebit.Size = new Size(135, 20);
            previousDebit.TabIndex = 9;
            previousDebit.Text = "Previous Debit : {}";
            // 
            // ReviewConsumptionDebitRecordForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(767, 407);
            Controls.Add(previousDebit);
            Controls.Add(reject);
            Controls.Add(verify);
            Controls.Add(rejectionReason);
            Controls.Add(image);
            Controls.Add(debitLb);
            Controls.Add(correctedBy);
            Controls.Add(submittedBy);
            Controls.Add(customerName);
            Controls.Add(headerLb);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReviewConsumptionDebitRecordForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ReviewConsumptionDebitRecordForm";
            ((System.ComponentModel.ISupportInitialize)image).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label headerLb;
        private Label customerName;
        private Label submittedBy;
        private Label correctedBy;
        private Label debitLb;
        private PictureBox image;
        private TextBox rejectionReason;
        private Button verify;
        private Button reject;
        private Label previousDebit;
    }
}