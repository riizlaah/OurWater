using OurWaterDesktop.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class ManageUsersForm : Form
    {
        bool editMode = false;
        System.Windows.Forms.Timer debouncer;
        private readonly MainForm main;
        public ManageUsersForm(MainForm m)
        {
            main = m;
            debouncer = new System.Windows.Forms.Timer { Interval = 500 };
            debouncer.Tick += (s, e) =>
            {
                debouncer.Stop();
                RefreshData(search.Text);
            };
            InitializeComponent();
            Helper.GenerateTableColumns(table1, new[] { "Id", "Name", "Username", "Role", "Address" }, new[] { "Id", "Fullname", "Username", "Role", "Address" });
            roles.DataSource = new string[] { "admin", "officer", "customer" };
            filterRoles.DataSource = new string[] { "all", "admin", "officer", "customer" };
            RefreshData();
            ToggleInput(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            main.Show();
        }

        async public Task RefreshData(string searchStr = "")
        {
            var url = "users";
            if (searchStr.Trim() != "")
            {
                url += "?search=" + UrlEncoder.Default.Encode(searchStr);
                if (filterRoles.SelectedIndex > 0) url += url += "?role=" + UrlEncoder.Default.Encode(filterRoles.Text);
            }
            else
            {
                if (filterRoles.SelectedIndex > 0) url += "?role=" + UrlEncoder.Default.Encode(filterRoles.Text);
            }
            var (success, result) = await Helper.JsonReq<object, List<UserRes>>(url);
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
            }
            table1.DataSource = result.data;
        }

        private void ToggleInput(bool enabled, bool clear = false)
        {
            insertBtn.Visible = !enabled;
            editBtn.Visible = !enabled;
            deleteBtn.Visible = !enabled;
            table1.Enabled = !enabled;

            username.ReadOnly = !enabled;
            fullname.ReadOnly = !enabled;
            roles.Enabled = enabled;
            password.ReadOnly = !enabled;
            confirmPassword.ReadOnly = !enabled;
            togglePassword.Enabled = enabled;
            address.ReadOnly = !enabled;

            if (clear)
            {
                username.Text = "";
                fullname.Text = "";
                password.Text = "";
                confirmPassword.Text = "";
                address.Text = "";
                roles.SelectedIndex = -1;
            }

            cancelBtn.Visible = enabled;
            saveBtn.Visible = enabled;
        }

        private UserRes? GetSelected()
        {
            if (table1.SelectedCells.Count == 0) return null;
            return table1.SelectedCells[0].OwningRow.DataBoundItem as UserRes;
        }

        private void OnInsert(object sender, EventArgs e)
        {
            editMode = false;
            ToggleInput(true, true);
        }

        private void OnEdit(object sender, EventArgs e)
        {
            editMode = true;
            ToggleInput(true);
        }

        private void OnDelete(object sender, EventArgs e)
        {
            var item = GetSelected();
            if (item == null)
            {
                MessageBox.Show("Please select one row");
                return;
            }
            if (MessageBox.Show($"Are you sure want to delete '{item.fullname}'?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Delete(item.id);
            }
        }


        private void OnCancel(object sender, EventArgs e)
        {
            ToggleInput(false, true);
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (username.Text.Trim() == "")
            {
                MessageBox.Show("Username is required");
                return;
            }
            if (fullname.Text.Trim() == "")
            {
                MessageBox.Show("Full Name is required");
                return;
            }
            if (roles.Text.Trim() == "")
            {
                MessageBox.Show("Username Role is required");
                return;
            }

            if (address.Text.Trim() == "")
            {
                MessageBox.Show("Address is required");
                return;
            }
            if (editMode)
            {
                if (password.Text.Length > 0)
                {
                    if (password.Text.Length < 8)
                    {
                        MessageBox.Show("Password length must be 8 character or more");
                        return;
                    }
                    if (confirmPassword.Text != password.Text)
                    {
                        MessageBox.Show("Confirmation password must be same");
                        return;
                    }
                }
                var row = GetSelected();
                if (row == null) return;
                Update(row.id);
            }
            else
            {
                if (password.Text.Length < 8)
                {
                    MessageBox.Show("Password length must be 8 character or more");
                    return;
                }
                if (confirmPassword.Text != password.Text)
                {
                    MessageBox.Show("Confirmation password must be same");
                    return;
                }
                Create();
            }
        }

        private void OnTrySearch(object sender, EventArgs e)
        {
            debouncer.Stop();
            debouncer.Start();
        }


        async private Task Create()
        {
            var (success, result) = await Helper.JsonReq<UserReq, object>($"users", new UserReq
            {
                username = username.Text.Trim(),
                fullname = fullname.Text.Trim(),
                password = password.Text,
                role = roles.Text,
                address = address.Text.Trim(),
            }, "post");
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            ToggleInput(false, true);
            RefreshData();
        }

        async private Task Update(int id)
        {
            var (success, result) = await Helper.JsonReq<UserReq, object>($"users/{id}", new UserReq
            {
                username = username.Text.Trim(),
                fullname = fullname.Text.Trim(),
                password = password.Text,
                role = roles.Text,
                address = address.Text.Trim(),
            }, "put");
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            ToggleInput(false, true);
            RefreshData();
        }

        async private Task Delete(int id)
        {
            var (success, result) = await Helper.JsonReq<object, object>($"users/{id}", method: "delete");
            if (!success)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            ToggleInput(false, true);
            RefreshData();
        }

        private void OnCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var row = GetSelected();
            if (row == null) return;
            username.Text = row.username;
            fullname.Text = row.fullname;
            roles.Text = row.role;
            address.Text = row.address;
        }

        private void OnTogglePassword(object sender, EventArgs e)
        {
            password.PasswordChar = togglePassword.Checked ? '\0' : '*';
            confirmPassword.PasswordChar = togglePassword.Checked ? '\0' : '*';
        }

        private void OnRoleFilterChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }


    public class UserRes
    {
        public int id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string role { get; set; }
        public string address { get; set; }
    }

    public class UserReq
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string address { get; set; }
    }

}
