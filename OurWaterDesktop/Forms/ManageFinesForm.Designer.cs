namespace OurWaterDesktop.Forms
{
    partial class ManageFinesForm
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
            table1 = new DataGridView();
            deleteBtn = new Button();
            editBtn = new Button();
            insertBtn = new Button();
            cancelBtn = new Button();
            saveBtn = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            startDay = new NumericUpDown();
            endDay = new NumericUpDown();
            amount = new TextBox();
            defined = new RadioButton();
            continuous = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)table1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)startDay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)endDay).BeginInit();
            SuspendLayout();
            // 
            // table1
            // 
            table1.AllowUserToAddRows = false;
            table1.AllowUserToDeleteRows = false;
            table1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            table1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table1.Location = new Point(12, 76);
            table1.Name = "table1";
            table1.ReadOnly = true;
            table1.RowHeadersWidth = 51;
            table1.Size = new Size(421, 338);
            table1.TabIndex = 0;
            table1.CellClick += OnCellClicked;
            table1.CellFormatting += OnCellFormatting;
            // 
            // deleteBtn
            // 
            deleteBtn.Location = new Point(339, 40);
            deleteBtn.Name = "deleteBtn";
            deleteBtn.Size = new Size(94, 29);
            deleteBtn.TabIndex = 1;
            deleteBtn.Text = "Delete";
            deleteBtn.UseVisualStyleBackColor = true;
            deleteBtn.Click += OnDelete;
            // 
            // editBtn
            // 
            editBtn.Location = new Point(239, 40);
            editBtn.Name = "editBtn";
            editBtn.Size = new Size(94, 29);
            editBtn.TabIndex = 2;
            editBtn.Text = "Edit";
            editBtn.UseVisualStyleBackColor = true;
            editBtn.Click += OnEdit;
            // 
            // insertBtn
            // 
            insertBtn.Location = new Point(12, 40);
            insertBtn.Name = "insertBtn";
            insertBtn.Size = new Size(135, 29);
            insertBtn.TabIndex = 3;
            insertBtn.Text = "New";
            insertBtn.UseVisualStyleBackColor = true;
            insertBtn.Click += OnInsert;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(339, 511);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(94, 44);
            cancelBtn.TabIndex = 5;
            cancelBtn.Text = "Cancel";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += OnCancel;
            // 
            // saveBtn
            // 
            saveBtn.Location = new Point(339, 426);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(94, 43);
            saveBtn.TabIndex = 4;
            saveBtn.Text = "Save";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += OnSave;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 428);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 8;
            label2.Text = "Start Day";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 461);
            label3.Name = "label3";
            label3.Size = new Size(64, 20);
            label3.TabIndex = 10;
            label3.Text = "End Day";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 531);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 12;
            label4.Text = "Amount";
            // 
            // startDay
            // 
            startDay.Location = new Point(140, 426);
            startDay.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            startDay.Name = "startDay";
            startDay.Size = new Size(150, 27);
            startDay.TabIndex = 13;
            startDay.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // endDay
            // 
            endDay.Location = new Point(140, 458);
            endDay.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            endDay.Name = "endDay";
            endDay.Size = new Size(150, 27);
            endDay.TabIndex = 14;
            endDay.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // amount
            // 
            amount.Location = new Point(115, 528);
            amount.Name = "amount";
            amount.Size = new Size(175, 27);
            amount.TabIndex = 15;
            // 
            // defined
            // 
            defined.AutoSize = true;
            defined.Checked = true;
            defined.Location = new Point(115, 465);
            defined.Name = "defined";
            defined.Size = new Size(17, 16);
            defined.TabIndex = 16;
            defined.TabStop = true;
            defined.UseVisualStyleBackColor = true;
            defined.CheckedChanged += OnDefinedCheckStateChanged;
            // 
            // continuous
            // 
            continuous.AutoSize = true;
            continuous.Location = new Point(115, 493);
            continuous.Name = "continuous";
            continuous.Size = new Size(104, 24);
            continuous.TabIndex = 17;
            continuous.Text = "Continuous";
            continuous.UseVisualStyleBackColor = true;
            // 
            // ManageFinesForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(444, 567);
            Controls.Add(continuous);
            Controls.Add(defined);
            Controls.Add(amount);
            Controls.Add(endDay);
            Controls.Add(startDay);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cancelBtn);
            Controls.Add(saveBtn);
            Controls.Add(insertBtn);
            Controls.Add(editBtn);
            Controls.Add(deleteBtn);
            Controls.Add(table1);
            MaximizeBox = false;
            Name = "ManageFinesForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manage Fine Rules";
            ((System.ComponentModel.ISupportInitialize)table1).EndInit();
            ((System.ComponentModel.ISupportInitialize)startDay).EndInit();
            ((System.ComponentModel.ISupportInitialize)endDay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView table1;
        private Button deleteBtn;
        private Button editBtn;
        private Button insertBtn;
        private Button cancelBtn;
        private Button saveBtn;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown startDay;
        private NumericUpDown endDay;
        private TextBox amount;
        private RadioButton defined;
        private RadioButton continuous;
    }
}