namespace OurWaterDesktop.Views
{
    partial class ViewProductionDebitRecordsForm
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
            label1 = new Label();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel2 = new Panel();
            monthPicker = new ComboBox();
            yearInp = new NumericUpDown();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)yearInp).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(8, 8, 8, 16);
            label1.Size = new Size(363, 55);
            label1.TabIndex = 1;
            label1.Text = "View Production Debit Records";
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(12);
            panel1.Size = new Size(927, 592);
            panel1.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(12, 67);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(903, 513);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Controls.Add(monthPicker);
            panel2.Controls.Add(yearInp);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(903, 55);
            panel2.TabIndex = 3;
            // 
            // monthPicker
            // 
            monthPicker.DropDownStyle = ComboBoxStyle.DropDownList;
            monthPicker.FormattingEnabled = true;
            monthPicker.Location = new Point(593, 21);
            monthPicker.Name = "monthPicker";
            monthPicker.Size = new Size(151, 28);
            monthPicker.TabIndex = 3;
            monthPicker.SelectedIndexChanged += OnMonthChanged;
            // 
            // yearInp
            // 
            yearInp.Location = new Point(750, 22);
            yearInp.Name = "yearInp";
            yearInp.Size = new Size(150, 27);
            yearInp.TabIndex = 2;
            yearInp.ValueChanged += OnYearChanged;
            // 
            // ViewProductionDebitRecordsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(927, 592);
            Controls.Add(panel1);
            Name = "ViewProductionDebitRecordsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ViewProductionDebitRecordsForm";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)yearInp).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Panel panel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel2;
        private ComboBox monthPicker;
        private NumericUpDown yearInp;
    }
}