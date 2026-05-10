namespace OurWaterDesktop.UserControls
{
    partial class ProdDebitCard
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
            day = new Label();
            debitLb = new Label();
            SuspendLayout();
            // 
            // day
            // 
            day.AutoSize = true;
            day.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            day.Location = new Point(9, 9);
            day.Name = "day";
            day.Size = new Size(52, 41);
            day.TabIndex = 0;
            day.Text = "01";
            // 
            // debitLb
            // 
            debitLb.AutoSize = true;
            debitLb.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            debitLb.ForeColor = SystemColors.Highlight;
            debitLb.Location = new Point(9, 68);
            debitLb.Name = "debitLb";
            debitLb.Size = new Size(95, 28);
            debitLb.TabIndex = 1;
            debitLb.Text = "Debit : {}";
            // 
            // ProdDebitCard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(debitLb);
            Controls.Add(day);
            Name = "ProdDebitCard";
            Size = new Size(185, 103);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label day;
        private Label debitLb;
    }
}
