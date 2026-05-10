namespace OurWaterDesktop.Forms
{
    partial class SubmitProdDebit
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
            button1 = new Button();
            label1 = new Label();
            button2 = new Button();
            debit = new TextBox();
            label2 = new Label();
            datePicker = new DateTimePicker();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(204, 187);
            button1.Name = "button1";
            button1.Size = new Size(79, 27);
            button1.TabIndex = 0;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += onSave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(45, 64);
            label1.Name = "label1";
            label1.Size = new Size(41, 20);
            label1.TabIndex = 1;
            label1.Text = "Date";
            // 
            // button2
            // 
            button2.Location = new Point(12, 187);
            button2.Name = "button2";
            button2.Size = new Size(79, 27);
            button2.TabIndex = 3;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += onCancel;
            // 
            // debit
            // 
            debit.Location = new Point(123, 107);
            debit.Name = "debit";
            debit.Size = new Size(136, 27);
            debit.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(45, 110);
            label2.Name = "label2";
            label2.Size = new Size(46, 20);
            label2.TabIndex = 4;
            label2.Text = "Debit";
            // 
            // datePicker
            // 
            datePicker.Format = DateTimePickerFormat.Short;
            datePicker.Location = new Point(123, 64);
            datePicker.Name = "datePicker";
            datePicker.Size = new Size(136, 27);
            datePicker.TabIndex = 6;
            // 
            // SubmitProdDebit
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(295, 226);
            Controls.Add(datePicker);
            Controls.Add(debit);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(button1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SubmitProdDebit";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Submit Production Debit";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Button button2;
        private TextBox debit;
        private Label label2;
        private DateTimePicker datePicker;
    }
}