using OurWaterDesktop.Views;
using System.Net.Http.Json;
using System.Net.Sockets;

namespace OurWaterDesktop
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void OnTryLogin(object sender, EventArgs e)
        {
            if (username.Text.Trim() == "")
            {
                MessageBox.Show("Username is required");
                return;
            }
            if (password.Text.Trim() == "")
            {
                MessageBox.Show("Password is required");
                return;
            }
            TryLogin();
            
        }

        private async Task TryLogin()
        {
            var (isSuccess, result) = await Helper.JsonReq<LoginReq, LoginRes>("users/login", new LoginReq { username = username.Text, password = password.Text }, "post");
            if(!isSuccess || result.data == null)
            {
                MessageBox.Show(result.message, "Error");
                return;
            }
            Helper.CurrentSession = result.data;
            username.Text = "";
            password.Text = "";
            var window = new MainForm(this);
            Hide();
            window.Show();
        }
    }

    public class LoginReq
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }


    public class LoginRes
    {
        public string fullname { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string token { get; set; }
    }

}
