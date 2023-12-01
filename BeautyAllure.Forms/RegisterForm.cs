using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BeautyAllure.Forms
{
    public partial class RegisterForm : Form
    {
        private ErrorProvider errorProvider;

        public RegisterForm()
        {
            InitializeComponent();
            InitializeForm();
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (AreAllInputsValid())
            {
                try
                {
                    InsertUserData(txtName.Text, txtPhone.Text, txtEmail.Text, txtPassword.Text);
                    MessageBox.Show("Congratulations! Account was created successfully.", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TransitionToLoginForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during registration: " + ex.Message, "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void InitializeForm()
        {
            errorProvider = new ErrorProvider
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink
            };
            txtPassword.PasswordChar = '*';
            txtConfirmPassword.PasswordChar = '*';
        }

        private void InsertUserData(string name, string phone, string email, string password)
        {
            string connectionString = "server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO user (name, phone, email, password) VALUES (@Name, @Phone, @Email, @Password)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                cmd.ExecuteNonQuery();
            }
        }

        private bool AreAllInputsValid()
        {
            errorProvider.Clear();

            ValidateName();
            ValidatePhone();
            ValidateEmail();
            ValidatePassword();
            ValidateConfirmPassword();

            return !HasAnyValidationErrors();
        }

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider.SetError(txtName, "Name cannot be empty.");
            }
            else if (!IsNameValid(txtName.Text))
            {
                errorProvider.SetError(txtName, "Invalid name. Must be at least 3 characters, no special characters or numbers.");
            }
            else
            {
                errorProvider.SetError(txtName, "");
            }
        }

        private void ValidatePhone()
        {
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !IsValidPhone(txtPhone.Text))
            {
                errorProvider.SetError(txtPhone, "Invalid phone number. Must be 10 digits.");
            }
            else
            {
                errorProvider.SetError(txtPhone, "");
            }
        }

        private void ValidateEmail()
        {
            if (IsEmailAlreadyUsed(txtEmail.Text))
            {
                errorProvider.SetError(txtEmail, "Email is already associated with an account.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider.SetError(txtEmail, "Email cannot be empty.");
            }
            else if (!IsValidEmail(txtEmail.Text))
            {
                errorProvider.SetError(txtEmail, "Invalid email format.");
            }
            else
            {
                errorProvider.SetError(txtEmail, "");
            }
        }

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Password cannot be empty.");
            }
            else if (!IsValidPassword(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Password must be at least 6 characters.");
            }
            else
            {
                errorProvider.SetError(txtPassword, "");
            }
        }

        private void ValidateConfirmPassword()
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                errorProvider.SetError(txtConfirmPassword, "Confirm Password cannot be empty.");
            }
            else if (txtPassword.Text != txtConfirmPassword.Text)
            {
                errorProvider.SetError(txtConfirmPassword, "Passwords do not match.");
            }
            else
            {
                errorProvider.SetError(txtConfirmPassword, "");
            }
        }

        private bool IsNameValid(string name)
        {
            return name.Length >= 3 && Regex.IsMatch(name, "^[a-zA-Z]+$");
        }

        private bool IsValidPhone(string phone)
        {
            return phone.All(char.IsDigit) && phone.Length == 10;
        }

        private bool IsValidEmail(string email)
        {
            return email.Length >= 8 && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6;
        }

        private bool IsEmailAlreadyUsed(string email)
        {

            string connectionString = "server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM user WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private bool HasAnyValidationErrors()
        {
            return new[] { txtName, txtPhone, txtEmail, txtPassword, txtConfirmPassword }
                .Any(txt => !string.IsNullOrEmpty(errorProvider.GetError(txt)));
        }

        private bool RegisterNewUser()
        {
            try
            {
                InsertUserData(txtName.Text, txtPhone.Text, txtEmail.Text, txtPassword.Text);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during registration: " + ex.Message, "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void TransitionToLoginForm()
        {
            this.Close();
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            TransitionToLoginForm();
        }
    }
}
