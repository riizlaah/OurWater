namespace OurWaterDesktop.UserControls
{
    partial class BillCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.header = new System.Windows.Forms.Label();
            this.debit = new System.Windows.Forms.Label();
            this.amount = new System.Windows.Forms.Label();
            this.customerName = new System.Windows.Forms.Label();
            this.deadline = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(15, 25);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(206, 25);
            this.header.TabIndex = 0;
            this.header.Text = "2026-01-01 - Status";
            // 
            // debit
            // 
            this.debit.AutoSize = true;
            this.debit.Location = new System.Drawing.Point(17, 85);
            this.debit.Name = "debit";
            this.debit.Size = new System.Drawing.Size(73, 16);
            this.debit.TabIndex = 1;
            this.debit.Text = "Debit : 0 M³";
            // 
            // amount
            // 
            this.amount.AutoSize = true;
            this.amount.Location = new System.Drawing.Point(17, 113);
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(107, 16);
            this.amount.TabIndex = 2;
            this.amount.Text = "Amount : Rp5000";
            // 
            // customerName
            // 
            this.customerName.AutoSize = true;
            this.customerName.Location = new System.Drawing.Point(17, 141);
            this.customerName.Name = "customerName";
            this.customerName.Size = new System.Drawing.Size(120, 16);
            this.customerName.TabIndex = 3;
            this.customerName.Text = "Customer : {Name}";
            // 
            // deadline
            // 
            this.deadline.AutoSize = true;
            this.deadline.Location = new System.Drawing.Point(17, 170);
            this.deadline.Name = "deadline";
            this.deadline.Size = new System.Drawing.Size(135, 16);
            this.deadline.TabIndex = 4;
            this.deadline.Text = "Deadline : 2026-01-15";
            // 
            // BillCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.deadline);
            this.Controls.Add(this.customerName);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.debit);
            this.Controls.Add(this.header);
            this.Name = "BillCard";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Size = new System.Drawing.Size(455, 205);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Label debit;
        private System.Windows.Forms.Label amount;
        private System.Windows.Forms.Label customerName;
        private System.Windows.Forms.Label deadline;
    }
}
