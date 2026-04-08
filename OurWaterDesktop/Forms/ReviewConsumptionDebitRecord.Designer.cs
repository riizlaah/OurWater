namespace OurWaterDesktop.Forms
{
    partial class ReviewConsumptionDebitRecord
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customerName = new System.Windows.Forms.Label();
            this.customerAddr = new System.Windows.Forms.Label();
            this.inputtedByType = new System.Windows.Forms.GroupBox();
            this.inputtedByName = new System.Windows.Forms.Label();
            this.debit = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rejectionReason = new System.Windows.Forms.TextBox();
            this.correctedByGroup = new System.Windows.Forms.GroupBox();
            this.correctedByName = new System.Windows.Forms.Label();
            this.verify = new System.Windows.Forms.Button();
            this.reject = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.inputtedByType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.correctedByGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // date
            // 
            this.date.AutoSize = true;
            this.date.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date.Location = new System.Drawing.Point(23, 25);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(143, 29);
            this.date.TabIndex = 0;
            this.date.Text = "2026-01-01";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(25, 78);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(98, 16);
            this.status.TabIndex = 1;
            this.status.Text = "Status : {status}";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.customerAddr);
            this.groupBox1.Controls.Add(this.customerName);
            this.groupBox1.Location = new System.Drawing.Point(28, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Customer";
            // 
            // customerName
            // 
            this.customerName.AutoSize = true;
            this.customerName.Location = new System.Drawing.Point(17, 29);
            this.customerName.Name = "customerName";
            this.customerName.Size = new System.Drawing.Size(130, 16);
            this.customerName.TabIndex = 3;
            this.customerName.Text = "Name : Lorem Ipsum";
            // 
            // customerAddr
            // 
            this.customerAddr.AutoSize = true;
            this.customerAddr.Location = new System.Drawing.Point(17, 56);
            this.customerAddr.Name = "customerAddr";
            this.customerAddr.Size = new System.Drawing.Size(139, 16);
            this.customerAddr.TabIndex = 4;
            this.customerAddr.Text = "Address : Somewhere";
            // 
            // inputtedByType
            // 
            this.inputtedByType.Controls.Add(this.inputtedByName);
            this.inputtedByType.Location = new System.Drawing.Point(28, 268);
            this.inputtedByType.Name = "inputtedByType";
            this.inputtedByType.Size = new System.Drawing.Size(322, 65);
            this.inputtedByType.TabIndex = 5;
            this.inputtedByType.TabStop = false;
            this.inputtedByType.Text = "Inputted By";
            // 
            // inputtedByName
            // 
            this.inputtedByName.AutoSize = true;
            this.inputtedByName.Location = new System.Drawing.Point(17, 29);
            this.inputtedByName.Name = "inputtedByName";
            this.inputtedByName.Size = new System.Drawing.Size(130, 16);
            this.inputtedByName.TabIndex = 3;
            this.inputtedByName.Text = "Name : Lorem Ipsum";
            // 
            // debit
            // 
            this.debit.AutoSize = true;
            this.debit.Location = new System.Drawing.Point(25, 109);
            this.debit.Name = "debit";
            this.debit.Size = new System.Drawing.Size(70, 16);
            this.debit.TabIndex = 6;
            this.debit.Text = "Debit : 0M³";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureBox1.Location = new System.Drawing.Point(374, 78);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(322, 255);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // rejectionReason
            // 
            this.rejectionReason.Location = new System.Drawing.Point(28, 361);
            this.rejectionReason.Multiline = true;
            this.rejectionReason.Name = "rejectionReason";
            this.rejectionReason.Size = new System.Drawing.Size(322, 77);
            this.rejectionReason.TabIndex = 8;
            // 
            // correctedByGroup
            // 
            this.correctedByGroup.Controls.Add(this.correctedByName);
            this.correctedByGroup.Location = new System.Drawing.Point(374, 361);
            this.correctedByGroup.Name = "correctedByGroup";
            this.correctedByGroup.Size = new System.Drawing.Size(322, 77);
            this.correctedByGroup.TabIndex = 6;
            this.correctedByGroup.TabStop = false;
            this.correctedByGroup.Text = "Corrected By";
            // 
            // correctedByName
            // 
            this.correctedByName.AutoSize = true;
            this.correctedByName.Location = new System.Drawing.Point(17, 29);
            this.correctedByName.Name = "correctedByName";
            this.correctedByName.Size = new System.Drawing.Size(130, 16);
            this.correctedByName.TabIndex = 3;
            this.correctedByName.Text = "Name : Lorem Ipsum";
            // 
            // verify
            // 
            this.verify.Location = new System.Drawing.Point(256, 444);
            this.verify.Name = "verify";
            this.verify.Size = new System.Drawing.Size(94, 34);
            this.verify.TabIndex = 9;
            this.verify.Text = "Verify";
            this.verify.UseVisualStyleBackColor = true;
            this.verify.Click += new System.EventHandler(this.onVerify);
            // 
            // reject
            // 
            this.reject.Location = new System.Drawing.Point(28, 444);
            this.reject.Name = "reject";
            this.reject.Size = new System.Drawing.Size(94, 34);
            this.reject.TabIndex = 10;
            this.reject.Text = "Reject";
            this.reject.UseVisualStyleBackColor = true;
            this.reject.Click += new System.EventHandler(this.onReject);
            // 
            // ReviewConsumptionDebitRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 514);
            this.Controls.Add(this.reject);
            this.Controls.Add(this.verify);
            this.Controls.Add(this.correctedByGroup);
            this.Controls.Add(this.rejectionReason);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.debit);
            this.Controls.Add(this.inputtedByType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.date);
            this.Name = "ReviewConsumptionDebitRecord";
            this.Text = "ReviewConsumptionDebitRecord";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.inputtedByType.ResumeLayout(false);
            this.inputtedByType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.correctedByGroup.ResumeLayout(false);
            this.correctedByGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label customerName;
        private System.Windows.Forms.Label customerAddr;
        private System.Windows.Forms.GroupBox inputtedByType;
        private System.Windows.Forms.Label inputtedByName;
        private System.Windows.Forms.Label debit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox rejectionReason;
        private System.Windows.Forms.GroupBox correctedByGroup;
        private System.Windows.Forms.Label correctedByName;
        private System.Windows.Forms.Button verify;
        private System.Windows.Forms.Button reject;
    }
}