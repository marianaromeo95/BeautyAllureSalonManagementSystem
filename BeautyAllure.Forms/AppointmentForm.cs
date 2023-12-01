using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeautyAllure.Forms
{
    public partial class AppointmentForm : Form
    {
        private DashboardForm dashboardForm;
        public AppointmentForm(DashboardForm dashboard)
        {
            InitializeComponent();
            InitializeListView();
            PopulateComboBox();
            cbxServiceSelection.SelectedIndexChanged += new System.EventHandler(this.cbxServiceSelection_SelectedIndexChanged);
            dtpDate.ValueChanged += dtp_ValueChanged;
            dtpTime.ValueChanged += dtp_ValueChanged;
            dtpDate.Value = DateTime.Now;
            dashboardForm = dashboard;
        }


        private void PopulateComboBox()
        {
            MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;");
            MySqlCommand cmd = new MySqlCommand("SELECT ServiceId, Type, Duration, Price FROM service", connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int serviceId = reader.GetInt32(0);
                    string serviceType = reader.GetString(1);
                    int duration = reader.GetInt32(2);
                    decimal price = reader.GetDecimal(3);

                    ServiceItem item = new ServiceItem(serviceId, serviceType, duration, price);
                    cbxServiceSelection.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void InitializeListView()
        {
            // Clear existing items and columns
            lvwAppointment.Items.Clear();
            lvwAppointment.Columns.Clear();

            // Add a single column to the ListView
            lvwAppointment.Columns.Add("Details", -2, HorizontalAlignment.Left);

            // Ensure the ListView shows the headers and the items are in details view
            lvwAppointment.View = View.Details;
            lvwAppointment.HeaderStyle = ColumnHeaderStyle.None; // No column header is needed
            lvwAppointment.FullRowSelect = true;
        }

        private void cbxServiceSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the ListView before adding new selection
            lvwAppointment.Items.Clear();

            if (cbxServiceSelection.SelectedItem is ServiceItem selectedService)
            {
                // Add each piece of information as a new item with a header in bold
                AddListViewItem("Service Type", selectedService.ServiceType);
                AddListViewItem("Duration", $"{selectedService.Duration} mins");
                AddListViewItem("Price", $"${selectedService.Price}");
                AddListViewItem("Date", dtpDate.Value.ToShortDateString()); // Default date
                AddListViewItem("Time", dtpTime.Value.ToShortTimeString()); // Default time
            }
        }

        private void AddListViewItem(string header, string content)
        {
            var headerItem = new ListViewItem(header) { Font = new Font(lvwAppointment.Font, FontStyle.Bold) };
            lvwAppointment.Items.Add(headerItem);

            var contentItem = new ListViewItem(content);
            lvwAppointment.Items.Add(contentItem);
        }

        private void PopulateListViewWithServiceDetails()
        {
            lvwAppointment.Items.Clear();

            if (cbxServiceSelection.SelectedItem is ServiceItem selectedService)
            {
                AddListViewItem("Service Type", selectedService.ServiceType);
                AddListViewItem("Duration", $"{selectedService.Duration} mins");
                AddListViewItem("Price", $"${selectedService.Price}");
                AddListViewItem("Date", dtpDate.Value.ToShortDateString());
                AddListViewItem("Time", dtpTime.Value.ToShortTimeString());
            }
        }

        private bool InsertAppointment(int userId, int serviceId, string appointmentDate, string appointmentTime)
        {
            using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;"))
            {
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO appointment (Date, Time, User_UserId, Service_ServiceId) VALUES (@Date, @Time, @UserId, @ServiceId)", connection))
                {
                    cmd.Parameters.AddWithValue("@Date", appointmentDate);
                    cmd.Parameters.AddWithValue("@Time", appointmentTime);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Appointment confirmed and saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Failed to save appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ServiceItem selectedService = cbxServiceSelection.SelectedItem as ServiceItem;
            if (selectedService == null)
            {
                MessageBox.Show("Please select a service before confirming.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmation dialog
            ConfirmAndInsertAppointment(selectedService);
        }

        private void ConfirmAndInsertAppointment(ServiceItem selectedService)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to confirm this appointment?",
                                                "Confirm Appointment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string selectedDate = dtpDate.Value.ToString("yyyy-MM-dd");
                string selectedTime = dtpTime.Value.ToString("HH:mm:ss");

                if (InsertAppointment(UserContext.UserId, selectedService.ServiceId, selectedDate, selectedTime))
                {
                    dashboardForm.RefreshAppointmentsDisplay(); // Update the dashboard
                    this.Close(); // Close the AppointmentForm
                }
            }
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            if (cbxServiceSelection.SelectedItem != null)
            {
                PopulateListViewWithServiceDetails();
            }
        }
    }

   
}
