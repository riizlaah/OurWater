using OurWaterDesktop.Forms;
using OurWaterDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurWaterDesktop
{
    public partial class Login : Form
    {
        private readonly OurWater dbc;
        public Login()
        {
            dbc = new OurWater();
            InitializeComponent();
        }

        private void onTryLogin(object sender, EventArgs e)
        {
            if(username.Text.Length == 0)
            {
                MessageBox.Show("Username is required");
                return;
            }
            if (password.Text.Length == 0)
            {
                MessageBox.Show("Password is required");
                return;
            }
            var user = dbc.Users.FirstOrDefault(u => u.username == username.Text && u.password == password.Text);

            if(user == null)
            {
                MessageBox.Show("Credentials invalid");
                return;
            }
            dbc.user = user;
            var window = new Dashboard(dbc, this);
            Hide();
            window.Show();
            username.Text = "";
            password.Text = "";
        }
    }
}
