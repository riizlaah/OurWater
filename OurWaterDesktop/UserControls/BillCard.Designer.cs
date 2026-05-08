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
            headerLb = new Label();
            customerName = new Label();
            debitLb = new Label();
            totalAmount = new Label();
            deadline = new Label();
            SuspendLayout();
            // 
            // headerLb
            // 
            headerLb.AutoSize = true;
            headerLb.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            headerLb.Location = new Point(10, 10);
            headerLb.Name = "headerLb";
            headerLb.Size = new Size(173, 28);
            headerLb.TabIndex = 0;
            headerLb.Text = "{date} - ({status})";
            // 
            // customerName
            // 
            customerName.AutoSize = true;
            customerName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            customerName.Location = new Point(10, 78);
            customerName.Name = "customerName";
            customerName.Size = new Size(159, 23);
            customerName.TabIndex = 1;
            customerName.Text = "Customer Name : {}";
            // 
            // debitLb
            // 
            debitLb.AutoSize = true;
            debitLb.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            debitLb.Location = new Point(10, 115);
            debitLb.Name = "debitLb";
            debitLb.Size = new Size(75, 23);
            debitLb.TabIndex = 2;
            debitLb.Text = "Debit : {}";
            // 
            // totalAmount
            // 
            totalAmount.AutoSize = true;
            totalAmount.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            totalAmount.Location = new Point(10, 151);
            totalAmount.Name = "totalAmount";
            totalAmount.Size = new Size(137, 23);
            totalAmount.TabIndex = 3;
            totalAmount.Text = "Total Amount : {}";
            // 
            // deadline
            // 
            deadline.AutoSize = true;
            deadline.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            deadline.Location = new Point(10, 191);
            deadline.Name = "deadline";
            deadline.Size = new Size(101, 23);
            deadline.TabIndex = 4;
            deadline.Text = "Deadline : {}";
            // 
            // BillCard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(deadline);
            Controls.Add(totalAmount);
            Controls.Add(debitLb);
            Controls.Add(customerName);
            Controls.Add(headerLb);
            Margin = new Padding(8);
            Name = "BillCard";
            Size = new Size(353, 233);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label headerLb;
        private Label customerName;
        private Label debitLb;
        private Label totalAmount;
        private Label deadline;
    }
}
