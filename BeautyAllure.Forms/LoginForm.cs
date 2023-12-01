using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace BeautyAllure.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            txbPassword.PasswordChar = '*';
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            OpenRegisterForm();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            AttemptLogin();
        }

        private void OpenRegisterForm()
        {
            this.Hide();
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
            this.Show(); // This line will be executed after RegisterForm is closed
        }

        private void AttemptLogin()
        {
            var email = txbEmail.Text;
            var password = txbPassword.Text;

            if (!AreFieldsFilled(email, password))
            {
                MessageBox.Show("Email and Password fields cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int userId = AuthenticateUser(email, password);
            if (userId > 0)
            {
                UserContext.SetCurrentUser(userId, email);
                OpenDashboard();
            }
            else
            {
                MessageBox.Show("Wrong email or password! Please try again or register for a new account.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool AreFieldsFilled(string email, string password)
        {
            return !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password);
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }

        private int AuthenticateUser(string email, string password)
        {
            try
            {
                return GetUserCredentials(email, password); // Directly use the entered password
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }


        private int GetUserCredentials(string email, string password)
        {
            var connectionString = "server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;";
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand("SELECT UserId FROM user WHERE email = @Email AND password = @Password", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password); // Use the plain text password for comparison

                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0; // Return UserId or 0 if not found
            }
        }


        private void OpenDashboard()
        {
            HideLoginForm();
            var dashboardForm = new DashboardForm();
            dashboardForm.Show();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void HideLoginForm()
        {
            this.Hide();
        }
    }
}
