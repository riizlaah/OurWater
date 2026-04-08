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

namespace OurWaterDesktop.Forms
{
    public partial class Dashboard : Form
    {
        private readonly OurWater dbc;
        private readonly Login login;
        public Dashboard(OurWater _dbc, Login _login)
        {
            dbc = _dbc;
            login = _login;
            InitializeComponent();
        }

        private void manageUser(object sender, EventArgs e)
        {
            var window = new ManageUser(dbc, this);
            Hide();
            window.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            login.Show();
        }
    }
}
