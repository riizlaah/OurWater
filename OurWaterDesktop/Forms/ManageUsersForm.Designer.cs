namespace OurWaterDesktop.Forms
{
    partial class ManageUsersForm
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
            label1 = new Label();
            search = new TextBox();
            username = new TextBox();
            label2 = new Label();
            fullname = new TextBox();
            label3 = new Label();
            label4 = new Label();
            roles = new ComboBox();
            password = new TextBox();
            label5 = new Label();
            confirmPassword = new TextBox();
            label6 = new Label();
            address = new TextBox();
            label7 = new Label();
            togglePassword = new CheckBox();
            label8 = new Label();
            filterRoles = new ComboBox();
            phoneNumber = new TextBox();
            label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)table1).BeginInit();
            SuspendLayout();
            // 
            // table1
            // 
            table1.AllowUserToAddRows = false;
            table1.AllowUserToDeleteRows = false;
            table1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table1.Location = new Point(12, 76);
            table1.Name = "table1";
            table1.ReadOnly = true;
            table1.RowHeadersWidth = 51;
            table1.Size = new Size(720, 338);
            table1.TabIndex = 0;
            table1.CellClick += OnCellClicked;
            // 
            // deleteBtn
            // 
            deleteBtn.Location = new Point(638, 41);
            deleteBtn.Name = "deleteBtn";
            deleteBtn.Size = new Size(94, 29);
            deleteBtn.TabIndex = 1;
            deleteBtn.Text = "Delete";
            deleteBtn.UseVisualStyleBackColor = true;
            deleteBtn.Click += OnDelete;
            // 
            // editBtn
            // 
            editBtn.Location = new Point(538, 41);
            editBtn.Name = "editBtn";
            editBtn.Size = new Size(94, 29);
            editBtn.TabIndex = 2;
            editBtn.Text = "Edit";
            editBtn.UseVisualStyleBackColor = true;
            editBtn.Click += OnEdit;
            // 
            // insertBtn
            // 
            insertBtn.Location = new Point(538, 6);
            insertBtn.Name = "insertBtn";
            insertBtn.Size = new Size(194, 29);
            insertBtn.TabIndex = 3;
            insertBtn.Text = "Insert";
            insertBtn.UseVisualStyleBackColor = true;
            insertBtn.Click += OnInsert;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(15, 557);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(94, 29);
            cancelBtn.TabIndex = 5;
            cancelBtn.Text = "Cancel";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += OnCancel;
            // 
            // saveBtn
            // 
            saveBtn.Location = new Point(115, 557);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(94, 29);
            saveBtn.TabIndex = 4;
            saveBtn.Text = "Save";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += OnSave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 45);
            label1.Name = "label1";
            label1.Size = new Size(53, 20);
            label1.TabIndex = 6;
            label1.Text = "Search";
            // 
            // search
            // 
            search.Location = new Point(76, 43);
            search.Name = "search";
            search.Size = new Size(192, 27);
            search.TabIndex = 7;
            search.TextChanged += OnTrySearch;
            // 
            // username
            // 
            username.Location = new Point(126, 427);
            username.Name = "username";
            username.Size = new Size(230, 27);
            username.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 428);
            label2.Name = "label2";
            label2.Size = new Size(75, 20);
            label2.TabIndex = 8;
            label2.Text = "Username";
            // 
            // fullname
            // 
            fullname.Location = new Point(126, 460);
            fullname.Name = "fullname";
            fullname.Size = new Size(230, 27);
            fullname.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 461);
            label3.Name = "label3";
            label3.Size = new Size(76, 20);
            label3.TabIndex = 10;
            label3.Text = "Full Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 525);
            label4.Name = "label4";
            label4.Size = new Size(39, 20);
            label4.TabIndex = 12;
            label4.Text = "Role";
            // 
            // roles
            // 
            roles.DropDownStyle = ComboBoxStyle.DropDownList;
            roles.FormattingEnabled = true;
            roles.Location = new Point(126, 523);
            roles.Name = "roles";
            roles.Size = new Size(118, 28);
            roles.TabIndex = 13;
            // 
            // password
            // 
            password.Location = new Point(510, 427);
            password.Name = "password";
            password.PasswordChar = '*';
            password.Size = new Size(222, 27);
            password.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(377, 430);
            label5.Name = "label5";
            label5.Size = new Size(70, 20);
            label5.TabIndex = 14;
            label5.Text = "Password";
            // 
            // confirmPassword
            // 
            confirmPassword.Location = new Point(510, 460);
            confirmPassword.Name = "confirmPassword";
            confirmPassword.PasswordChar = '*';
            confirmPassword.Size = new Size(222, 27);
            confirmPassword.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(377, 463);
            label6.Name = "label6";
            label6.Size = new Size(127, 20);
            label6.TabIndex = 16;
            label6.Text = "Confirm Password";
            // 
            // address
            // 
            address.Location = new Point(510, 528);
            address.Multiline = true;
            address.Name = "address";
            address.Size = new Size(222, 58);
            address.TabIndex = 19;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(377, 531);
            label7.Name = "label7";
            label7.Size = new Size(62, 20);
            label7.TabIndex = 18;
            label7.Text = "Address";
            // 
            // togglePassword
            // 
            togglePassword.AutoSize = true;
            togglePassword.Location = new Point(510, 494);
            togglePassword.Name = "togglePassword";
            togglePassword.Size = new Size(132, 24);
            togglePassword.TabIndex = 20;
            togglePassword.Text = "Show Password";
            togglePassword.UseVisualStyleBackColor = true;
            togglePassword.CheckedChanged += OnTogglePassword;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 12);
            label8.Name = "label8";
            label8.Size = new Size(39, 20);
            label8.TabIndex = 21;
            label8.Text = "Role";
            // 
            // filterRoles
            // 
            filterRoles.FormattingEnabled = true;
            filterRoles.Location = new Point(76, 9);
            filterRoles.Name = "filterRoles";
            filterRoles.Size = new Size(192, 28);
            filterRoles.TabIndex = 22;
            filterRoles.SelectedIndexChanged += OnRoleFilterChanged;
            // 
            // phoneNumber
            // 
            phoneNumber.Location = new Point(126, 492);
            phoneNumber.Name = "phoneNumber";
            phoneNumber.Size = new Size(230, 27);
            phoneNumber.TabIndex = 24;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 493);
            label9.Name = "label9";
            label9.Size = new Size(108, 20);
            label9.TabIndex = 23;
            label9.Text = "Phone Number";
            // 
            // ManageUsersForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(744, 598);
            Controls.Add(phoneNumber);
            Controls.Add(label9);
            Controls.Add(filterRoles);
            Controls.Add(label8);
            Controls.Add(togglePassword);
            Controls.Add(address);
            Controls.Add(label7);
            Controls.Add(confirmPassword);
            Controls.Add(label6);
            Controls.Add(password);
            Controls.Add(label5);
            Controls.Add(roles);
            Controls.Add(label4);
            Controls.Add(fullname);
            Controls.Add(label3);
            Controls.Add(username);
            Controls.Add(label2);
            Controls.Add(search);
            Controls.Add(label1);
            Controls.Add(cancelBtn);
            Controls.Add(saveBtn);
            Controls.Add(insertBtn);
            Controls.Add(editBtn);
            Controls.Add(deleteBtn);
            Controls.Add(table1);
            MaximizeBox = false;
            MaximumSize = new Size(762, 645);
            MinimumSize = new Size(762, 645);
            Name = "ManageUsersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manage Users";
            ((System.ComponentModel.ISupportInitialize)table1).EndInit();
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
        private Label label1;
        private TextBox search;
        private TextBox username;
        private Label label2;
        private TextBox fullname;
        private Label label3;
        private Label label4;
        private ComboBox roles;
        private TextBox password;
        private Label label5;
        private TextBox confirmPassword;
        private Label label6;
        private TextBox address;
        private Label label7;
        private CheckBox togglePassword;
        private Label label8;
        private ComboBox filterRoles;
        private TextBox phoneNumber;
        private Label label9;
    }
}