namespace OurWaterDesktop.UserControls
{
    partial class DebitRecord
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
            this.inputtedBy = new System.Windows.Forms.Label();
            this.customerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.AutoSize = true;
            this.header.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(16, 16);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(206, 25);
            this.header.TabIndex = 0;
            this.header.Text = "2026-01-01 - Status";
            // 
            // debit
            // 
            this.debit.AutoSize = true;
            this.debit.Location = new System.Drawing.Point(18, 76);
            this.debit.Name = "debit";
            this.debit.Size = new System.Drawing.Size(70, 16);
            this.debit.TabIndex = 1;
            this.debit.Text = "Debit : 0M³";
            // 
            // inputtedBy
            // 
            this.inputtedBy.AutoSize = true;
            this.inputtedBy.Location = new System.Drawing.Point(18, 102);
            this.inputtedBy.Name = "inputtedBy";
            this.inputtedBy.Size = new System.Drawing.Size(129, 16);
            this.inputtedBy.TabIndex = 2;
            this.inputtedBy.Text = "Inputted By : {Name}";
            // 
            // customerName
            // 
            this.customerName.AutoSize = true;
            this.customerName.Location = new System.Drawing.Point(18, 131);
            this.customerName.Name = "customerName";
            this.customerName.Size = new System.Drawing.Size(120, 16);
            this.customerName.TabIndex = 3;
            this.customerName.Text = "Customer : {Name}";
            // 
            // DebitRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.customerName);
            this.Controls.Add(this.inputtedBy);
            this.Controls.Add(this.debit);
            this.Controls.Add(this.header);
            this.Name = "DebitRecord";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Size = new System.Drawing.Size(327, 172);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Label debit;
        private System.Windows.Forms.Label inputtedBy;
        private System.Windows.Forms.Label customerName;
    }
}
