namespace OurWaterDesktop.Views
{
    partial class MainForm
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
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            greetLb = new Label();
            dateTimeLb = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(322, 332);
            button1.Name = "button1";
            button1.Size = new Size(153, 59);
            button1.TabIndex = 0;
            button1.Text = "View Water Usage";
            button1.UseVisualStyleBackColor = true;
            button1.Click += OnViewWaterUsage;
            // 
            // button2
            // 
            button2.Location = new Point(123, 242);
            button2.Name = "button2";
            button2.Size = new Size(153, 59);
            button2.TabIndex = 1;
            button2.Text = "View Production Debit Record";
            button2.UseVisualStyleBackColor = true;
            button2.Click += OnViewProdDebitRecs;
            // 
            // button3
            // 
            button3.Location = new Point(123, 332);
            button3.Name = "button3";
            button3.Size = new Size(153, 59);
            button3.TabIndex = 2;
            button3.Text = "Submit Production Debit Record";
            button3.UseVisualStyleBackColor = true;
            button3.Click += OnSubmitProdDebitRec;
            // 
            // button4
            // 
            button4.Location = new Point(123, 162);
            button4.Name = "button4";
            button4.Size = new Size(153, 59);
            button4.TabIndex = 3;
            button4.Text = "View Customer Bills";
            button4.UseVisualStyleBackColor = true;
            button4.Click += OnViewCustomerBills;
            // 
            // button5
            // 
            button5.Location = new Point(322, 162);
            button5.Name = "button5";
            button5.Size = new Size(153, 59);
            button5.TabIndex = 4;
            button5.Text = "Manage Users";
            button5.UseVisualStyleBackColor = true;
            button5.Click += OnManageUsers;
            // 
            // button6
            // 
            button6.Location = new Point(322, 242);
            button6.Name = "button6";
            button6.Size = new Size(153, 59);
            button6.TabIndex = 5;
            button6.Text = "Settings Fine Rule";
            button6.UseVisualStyleBackColor = true;
            button6.Click += OnSettingFineRules;
            // 
            // button7
            // 
            button7.Location = new Point(123, 83);
            button7.Name = "button7";
            button7.Size = new Size(352, 59);
            button7.TabIndex = 6;
            button7.Text = "View Consumption Debit Record";
            button7.UseVisualStyleBackColor = true;
            button7.Click += OnViewConsDebitRec;
            // 
            // greetLb
            // 
            greetLb.Dock = DockStyle.Top;
            greetLb.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            greetLb.Location = new Point(8, 8);
            greetLb.Name = "greetLb";
            greetLb.Size = new Size(582, 25);
            greetLb.TabIndex = 7;
            greetLb.Text = "Hello {name}!";
            // 
            // dateTimeLb
            // 
            dateTimeLb.Dock = DockStyle.Bottom;
            dateTimeLb.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dateTimeLb.ForeColor = SystemColors.ControlDarkDark;
            dateTimeLb.Location = new Point(8, 417);
            dateTimeLb.Name = "dateTimeLb";
            dateTimeLb.Size = new Size(582, 23);
            dateTimeLb.TabIndex = 8;
            dateTimeLb.Text = "dddd, dd MM yyyy (HH:mm:ss)";
            dateTimeLb.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(598, 448);
            Controls.Add(dateTimeLb);
            Controls.Add(greetLb);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "MainForm";
            Padding = new Padding(8);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Label greetLb;
        private Label dateTimeLb;
    }
}