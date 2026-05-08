namespace OurWaterDesktop.UserControls
{
    partial class ConsumptionDebitCard
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
            submittedBy = new Label();
            location = new Label();
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
            // submittedBy
            // 
            submittedBy.AutoSize = true;
            submittedBy.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            submittedBy.Location = new Point(10, 151);
            submittedBy.Name = "submittedBy";
            submittedBy.Size = new Size(136, 23);
            submittedBy.TabIndex = 3;
            submittedBy.Text = "Submitted By : {}";
            // 
            // location
            // 
            location.AutoSize = true;
            location.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            location.Location = new Point(10, 191);
            location.Name = "location";
            location.Size = new Size(99, 23);
            location.TabIndex = 4;
            location.Text = "Location : {}";
            // 
            // ConsumptionDebitCard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(location);
            Controls.Add(submittedBy);
            Controls.Add(debitLb);
            Controls.Add(customerName);
            Controls.Add(headerLb);
            Margin = new Padding(8);
            Name = "ConsumptionDebitCard";
            Size = new Size(353, 233);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label headerLb;
        private Label customerName;
        private Label debitLb;
        private Label submittedBy;
        private Label location;
    }
}
