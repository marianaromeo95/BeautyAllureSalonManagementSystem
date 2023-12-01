using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeautyAllure.Forms
{
    public partial class ModifyAppointmentForm : Form
    {
        private int appointmentId;
        private DashboardForm dashboardForm;

        public ModifyAppointmentForm(int appointmentId, DashboardForm dashboard)
        {
            InitializeComponent();
            this.appointmentId = appointmentId;
            dtpDate.ValueChanged += dtp_ValueChanged;
            dtpTime.ValueChanged += dtp_ValueChanged;
            dashboardForm = dashboard;
            InitializeListView();
            PopulateAppointmentDetails();

            // Populate appointment details when the form is initialized

        }

        private void InitializeListView()
        {
            lvwAppointment.Items.Clear();
            lvwAppointment.Columns.Clear();
            lvwAppointment.Columns.Add("Details", -2, HorizontalAlignment.Left);
            lvwAppointment.View = View.Details;
            lvwAppointment.HeaderStyle = ColumnHeaderStyle.None;
            lvwAppointment.FullRowSelect = true;
        }

        private void PopulateAppointmentDetails()
        {
            using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;"))
            {
                string appointmentQuery = @"
            SELECT a.Service_ServiceId, a.Date, a.Time, s.Type, s.Duration, s.Price 
            FROM appointment a
            INNER JOIN service s ON a.Service_ServiceId = s.ServiceId
            WHERE a.AppointmentId = @AppointmentId";

                using (MySqlCommand cmd = new MySqlCommand(appointmentQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int serviceId = reader.GetInt32("Service_ServiceId");
                            DateTime date = reader.GetDateTime("Date");
                            TimeSpan time = (TimeSpan)reader["Time"];
                            string serviceType = reader.GetString("Type");
                            int duration = reader.GetInt32("Duration");
                            decimal price = reader.GetDecimal("Price");

                            PopulateComboBox(serviceId);
                            dtpDate.Value = date;
                            dtpTime.Value = DateTime.Today.Add(time);

                            PopulateListViewWithServiceDetails(serviceType, duration, price, date, time);
                        }
                    }
                }
            }
        }


        private void PopulateComboBox(int selectedServiceId)
        {
            // Populate the service selection ComboBox and set the selected service
            MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;");
            MySqlCommand cmd = new MySqlCommand("SELECT ServiceId, Type FROM service", connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int serviceId = reader.GetInt32(0);
                    string serviceType = reader.GetString(1);

                    ServiceItem item = new ServiceItem(serviceId, serviceType, 0, 0);
                    cbxServiceSelection.Items.Add(item);

                    // Set the selected service in the ComboBox
                    if (serviceId == selectedServiceId)
                    {
                        cbxServiceSelection.SelectedItem = item;
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }


        private void PopulateListViewWithServiceDetails(string serviceType, int duration, decimal price, DateTime date, TimeSpan time)
        {
            lvwAppointment.Items.Clear();

            AddListViewItem("Service Type", serviceType);
            AddListViewItem("Duration", $"{duration} mins");
            AddListViewItem("Price", $"${price:0.00}"); // Format the price to always show 2 decimal places
            AddListViewItem("Date", date.ToString("MM/dd/yyyy")); // Use a consistent date format
            AddListViewItem("Time", new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, time.Hours, time.Minutes, time.Seconds).ToString("hh:mm tt")); // Convert the TimeSpan to a DateTime to format it
        }
        private void AddListViewItem(string header, string content)
        {
            var headerItem = new ListViewItem(header) { Font = new Font(lvwAppointment.Font, FontStyle.Bold) };
            lvwAppointment.Items.Add(headerItem);

            var contentItem = new ListViewItem(content);
            lvwAppointment.Items.Add(contentItem);
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
            ConfirmAndUpdateAppointment(selectedService);
        }

        private void ConfirmAndUpdateAppointment(ServiceItem selectedService)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to update this appointment?",
                                                "Confirm Appointment Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string selectedDate = dtpDate.Value.ToString("yyyy-MM-dd");
                string selectedTime = dtpTime.Value.ToString("HH:mm:ss");

                // Update the appointment in the database
                if (UpdateAppointment(appointmentId, selectedService.ServiceId, selectedDate, selectedTime))
                {
                    dashboardForm.RefreshAppointmentsDisplay(); // Update the dashboard
                    this.Close(); // Close the ModifyAppointmentForm
                }
            }
        }

        private bool UpdateAppointment(int appointmentId, int serviceId, string appointmentDate, string appointmentTime)
        {
            using (MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=3306;database=beauty_allure_db;uid=root;"))
            {
                using (MySqlCommand cmd = new MySqlCommand("UPDATE appointment SET Date = @Date, Time = @Time, Service_ServiceId = @ServiceId WHERE AppointmentId = @AppointmentId", connection))
                {
                    cmd.Parameters.AddWithValue("@Date", appointmentDate);
                    cmd.Parameters.AddWithValue("@Time", appointmentTime);
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Appointment updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Failed to update appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            // Assuming cbxServiceSelection has the correct ServiceItem selected
            if (cbxServiceSelection.SelectedItem is ServiceItem selectedService)
            {
                PopulateListViewWithServiceDetails(
                    selectedService.ServiceType,
                    selectedService.Duration,
                    selectedService.Price,
                    dtpDate.Value,
                    dtpTime.Value.TimeOfDay // This gets the TimeSpan from the DateTime
                );
            }
        }
    }
}