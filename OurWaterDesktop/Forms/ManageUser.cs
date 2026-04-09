using OurWaterDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop.Forms
{
    public partial class ManageUser : Form
    {
        private readonly OurWater dbc;
        private readonly Dashboard dashb;
        bool editing = false;
        public ManageUser(OurWater ctx, Dashboard dash)
        {
            dbc = ctx;
            dashb = dash;
            InitializeComponent();
            Helper.BindTableColumns(table1, new[] { "Id", "Username", "Full Name" }, new[] { "id", "username", "fullname" });
            RefreshData();
            enableInputs(false);
        }

        private void RefreshData(string search = "")
        {
            if(search.Trim() != "")
            {
                table1.DataSource = dbc.Users.Where(u => DbFunctions.Like(u.username, "%" + search + "%") || DbFunctions.Like(u.fullname, "%"+ search +"%")).ToList();
            } else
            {
                table1.DataSource = dbc.Users.ToList();
            }
        }

        private void onCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            var user = GetSelected();
            if (user == null) return;
            username.Text = user.username;
            password.Text = user.password;
            fullname.Text = user.fullname;
            address.Text = user.address;
            roles.SelectedItem = user.role;
            edit.Show();
            delete.Show();
        }

        private User GetSelected()
        {
            if (table1.SelectedRows.Count == 0) return null;
            return table1.SelectedRows[0].DataBoundItem as User;
        }

        private void onTrySearch(object sender, EventArgs e)
        {
            RefreshData(searchInp.Text);
        }

        private void onCreate(object sender, EventArgs e)
        {
            enableInputs(true);
        }

        private void enableInputs(bool enabled)
        {
            username.ReadOnly = !enabled;
            password.ReadOnly = !enabled;
            fullname.ReadOnly = !enabled;
            address.ReadOnly = !enabled;
            roles.Enabled = enabled;

            save.Visible = enabled;
            cancel.Visible = enabled;
            table1.Enabled = !enabled;

            edit.Visible = !enabled;
            delete.Visible = !enabled;
        }

        private void onCancel(object sender, EventArgs e)
        {
            enableInputs(false);
        }

        private void onSave(object sender, EventArgs e)
        {
            if(username.Text.Trim() == "" || password.Text.Trim() == "" ||  fullname.Text.Trim() == "" || address.Text.Trim() == "")
            {
                MessageBox.Show("All data must be filled");
                return;
            }
            if(roles.SelectedItem == null)
            {
                MessageBox.Show("Role must be selected");
                return;
            }
            if(editing)
            {
                var user1 = GetSelected();
                if (user1 == null) return;
                if (dbc.Users.Any(u => u.username == username.Text && u.id != user1.id))
                {
                    MessageBox.Show("Username has been used");
                    return;
                }
                var user = dbc.Users.Find(user1.id);
                user.username = username.Text;
                user.fullname = fullname.Text;
                user.password = password.Text;
                user.address = address.Text;
                user.role = roles.SelectedItem.ToString();
                dbc.SaveChanges();
            } else
            {
                if(dbc.Users.Any(u => u.username == username.Text))
                {
                    MessageBox.Show("Username has been used");
                    return;
                }
                dbc.Users.Add(new User { username = username.Text, password = password.Text, fullname = fullname.Text, address = address.Text, role = roles.SelectedItem.ToString() });
                dbc.SaveChanges();
            }
            enableInputs(false);
            RefreshData();
            editing = false;
        }

        private void onDelete(object sender, EventArgs e)
        {
            var user = GetSelected();
            if (user == null) return;
            if(MessageBox.Show($"Are you sure want to delete '{user.fullname}'?", "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                dbc.Users.Remove(user);
                dbc.SaveChanges();
                RefreshData();
            }
        }

        private void onEdit(object sender, EventArgs e)
        {
            var user = GetSelected();
            if (user == null) return;
            editing = true;
            enableInputs(true);
        }

        protected override void OnClosed(EventArgs e)
        {
            dashb.Show();
        }
    }
}
