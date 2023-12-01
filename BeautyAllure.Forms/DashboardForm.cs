using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace BeautyAllure.Forms
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
            RefreshAppointmentsDisplay();
            dgvAppointments.DataSource = GetDataForCurrentUser();
            dgvAppointments.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvAppointments_CellFormatting);

        }

        private DataTable GetDataForCurrentUser()
        {
            // Connection string (adjust with your settings)
            string connString = "server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;";

            // SQL query to fetch data for the current user
            string query = "SELECT a.appointmentId, s.Type, a.Date, a.Time FROM appointment a " +
                           "INNER JOIN service s ON a.Service_ServiceId = s.ServiceId " +
                           "WHERE a.User_UserId = @UserId"; // Filter by current user ID

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserContext.UserId);
                    conn.Open();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

        private void btnNewAppointment_Click(object sender, EventArgs e)
        {
            using (AppointmentForm appointmentForm = new AppointmentForm(this))
            {
                appointmentForm.ShowDialog();
            }
            RefreshAppointmentsDisplay();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Clear the user context on logout
                UserContext.Clear();

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        private void dgvAppointments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Assuming that the Time column is named "Time" in your DataGridView
            if (dgvAppointments.Columns[e.ColumnIndex].Name == "Time" && e.Value != null)
            {
                TimeSpan time;
                if (TimeSpan.TryParse(e.Value.ToString(), out time))
                {
                    e.Value = DateTime.Today.Add(time).ToString("hh:mm tt");
                    e.FormattingApplied = true;
                }
            }
        }
        public void RefreshAppointmentsDisplay()
        {
            var data = GetDataForCurrentUser();
            dgvAppointments.DataSource = data;

            if (dgvAppointments.Columns.Contains("appointmentId"))
            {
                dgvAppointments.Columns["appointmentId"].Visible = false; // Hide appointmentId column
            }



            // Assuming you have two DataGridViewButtonColumns for Modify and Cancel actions
            if (!dgvAppointments.Columns.Contains("ModifyColumn"))
            {
                var modifyColumn = new DataGridViewButtonColumn();
                modifyColumn.Name = "ModifyColumn";
                modifyColumn.HeaderText = ""; // Set header text to be blank
                modifyColumn.Text = "Modify";
                modifyColumn.UseColumnTextForButtonValue = true;
                dgvAppointments.Columns.Add(modifyColumn);
            }

            if (!dgvAppointments.Columns.Contains("CancelColumn"))
            {
                var cancelColumn = new DataGridViewButtonColumn();
                cancelColumn.Name = "CancelColumn";
                cancelColumn.HeaderText = ""; // Set header text to be blank
                cancelColumn.Text = "Cancel";
                cancelColumn.UseColumnTextForButtonValue = true;
                dgvAppointments.Columns.Add(cancelColumn);
            }

            // Set the display index to move Modify and Cancel to the last positions
            dgvAppointments.Columns["ModifyColumn"].DisplayIndex = dgvAppointments.Columns.Count - 1;
            dgvAppointments.Columns["CancelColumn"].DisplayIndex = dgvAppointments.Columns.Count - 1;

            // Show or hide button columns based on whether there are appointments
            dgvAppointments.Columns["ModifyColumn"].Visible = data.Rows.Count > 0;
            dgvAppointments.Columns["CancelColumn"].Visible = data.Rows.Count > 0;
        }


        private void dgvAppointments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvAppointments.Columns[e.ColumnIndex].Name == "ModifyColumn")
                {
                    // Get the appointment ID from the selected row (assuming it's stored in the first cell)
                    int appointmentId = Convert.ToInt32(dgvAppointments.Rows[e.RowIndex].Cells["appointmentId"].Value);
                    // Call your method to modify the appointment
                    ModifyAppointment(appointmentId);
                }
                else if (dgvAppointments.Columns[e.ColumnIndex].Name == "CancelColumn")
                {
                    int appointmentId = Convert.ToInt32(dgvAppointments.Rows[e.RowIndex].Cells["appointmentId"].Value);
                    // Call your method to cancel the appointment
                    CancelAppointment(appointmentId);
                }
            }
        }

        private void ModifyAppointment(int appointmentId)
        {
            // Assuming you have a ModifyAppointmentForm
            using (ModifyAppointmentForm modifyForm = new ModifyAppointmentForm(appointmentId, this))
            {
                if (modifyForm.ShowDialog() == DialogResult.OK)
                {
                    // Handle any post-modification logic here
                    // For example, refresh the appointments display
                    RefreshAppointmentsDisplay();
                }
            }
        }

        private void CancelAppointment(int appointmentId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel this appointment?", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Perform the cancellation logic here (e.g., delete the appointment from the database)
                if (DeleteAppointmentFromDatabase(appointmentId))
                {
                    // Appointment successfully canceled
                    MessageBox.Show("Appointment canceled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshAppointmentsDisplay(); // Refresh the UI to reflect the changes
                }
                else
                {
                    // Handle cancellation failure (e.g., show an error message)
                    MessageBox.Show("Failed to cancel the appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool DeleteAppointmentFromDatabase(int appointmentId)
        {
            // Implement the logic to delete the appointment from the database here
            // Use the provided appointmentId to identify and delete the appointment

            // Return true if the deletion was successful, otherwise return false
            bool deletionSuccessful = false;

            // Example code to delete the appointment (adjust it based on your database structure)
            string connString = "server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;";
            string deleteQuery = "DELETE FROM appointment WHERE AppointmentId = @AppointmentId";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    conn.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Deletion was successful
                        deletionSuccessful = true;
                    }
                }
            }

            return deletionSuccessful;
        }

        private void dgvAppointments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
