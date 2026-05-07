using System.Net.Http.Json;
using System.Net.Sockets;

namespace OurWaterDesktop
{
    public partial class Login : Form
    {
        private const string ADDR = "http://localhost:5000/api/";
        private readonly HttpClient _httpClient;
        public Login()
        {
            _httpClient = new HttpClient();
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
            var res = await _httpClient.PostAsJsonAsync(ADDR + "users/login", new LoginReq { username = username.Text, password = password.Text });
            try
            {
                var res2 = await res.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                if (res2.data == null)
                {
                    MessageBox.Show(res2.message);
                    return;
                }
                MessageBox.Show($"Hello {res2.data.fullname}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }

    public class LoginReq
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }


    public class ApiResponse<T>
    {
        public T? data { get; set; }
        public string message { get; set; }
    }

    public class LoginResponse
    {
        public string fullname { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string token { get; set; }
    }

}
