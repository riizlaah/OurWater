namespace OurWaterDesktop.Forms
{
    partial class SubmitProductionDebitRecord
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
            this.label1 = new System.Windows.Forms.Label();
            this.save = new System.Windows.Forms.Button();
            this.debit = new System.Windows.Forms.TextBox();
            this.date = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Today Debit";
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(229, 204);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(62, 36);
            this.save.TabIndex = 2;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.onSave);
            // 
            // debit
            // 
            this.debit.Location = new System.Drawing.Point(112, 80);
            this.debit.Name = "debit";
            this.debit.Size = new System.Drawing.Size(87, 22);
            this.debit.TabIndex = 3;
            // 
            // date
            // 
            this.date.AutoSize = true;
            this.date.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date.Location = new System.Drawing.Point(21, 18);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(124, 25);
            this.date.TabIndex = 5;
            this.date.Text = "2026-01-01";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(24, 204);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(73, 36);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.onCancel);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(205, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "M³";
            // 
            // SubmitProductionDebitRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.date);
            this.Controls.Add(this.debit);
            this.Controls.Add(this.save);
            this.Controls.Add(this.label1);
            this.Name = "SubmitProductionDebitRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SubmitProductionDebitRecord";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.TextBox debit;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label2;
    }
}