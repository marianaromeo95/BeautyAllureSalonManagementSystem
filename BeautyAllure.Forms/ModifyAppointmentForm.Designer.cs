namespace BeautyAllure.Forms
{
    partial class ModifyAppointmentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNewAppointment = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxServiceSelection = new System.Windows.Forms.ComboBox();
            this.lblQuestion2 = new System.Windows.Forms.Label();
            this.lblQuestion3 = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lvwAppointment = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lblNewAppointment
            // 
            this.lblNewAppointment.AutoSize = true;
            this.lblNewAppointment.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewAppointment.Location = new System.Drawing.Point(155, 37);
            this.lblNewAppointment.Name = "lblNewAppointment";
            this.lblNewAppointment.Size = new System.Drawing.Size(337, 32);
            this.lblNewAppointment.TabIndex = 0;
            this.lblNewAppointment.Text = "Make a New Appointment";
            this.lblNewAppointment.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "1. What type of service?";
            // 
            // cbxServiceSelection
            // 
            this.cbxServiceSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxServiceSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxServiceSelection.FormattingEnabled = true;
            this.cbxServiceSelection.Location = new System.Drawing.Point(64, 171);
            this.cbxServiceSelection.Name = "cbxServiceSelection";
            this.cbxServiceSelection.Size = new System.Drawing.Size(273, 37);
            this.cbxServiceSelection.TabIndex = 2;
            // 
            // lblQuestion2
            // 
            this.lblQuestion2.AutoSize = true;
            this.lblQuestion2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestion2.Location = new System.Drawing.Point(38, 230);
            this.lblQuestion2.Name = "lblQuestion2";
            this.lblQuestion2.Size = new System.Drawing.Size(157, 29);
            this.lblQuestion2.TabIndex = 5;
            this.lblQuestion2.Text = "2. What date?";
            // 
            // lblQuestion3
            // 
            this.lblQuestion3.AutoSize = true;
            this.lblQuestion3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestion3.Location = new System.Drawing.Point(43, 345);
            this.lblQuestion3.Name = "lblQuestion3";
            this.lblQuestion3.Size = new System.Drawing.Size(156, 29);
            this.lblQuestion3.TabIndex = 6;
            this.lblQuestion3.Text = "3. What time?";
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "hh:mm tt";
            this.dtpTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(64, 398);
            this.dtpTime.MaxDate = new System.DateTime(2100, 1, 1, 12, 59, 0, 0);
            this.dtpTime.MinDate = new System.DateTime(2020, 1, 1, 1, 0, 0, 0);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(146, 35);
            this.dtpTime.TabIndex = 7;
            this.dtpTime.Value = new System.DateTime(2023, 11, 29, 0, 0, 0, 0);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.Location = new System.Drawing.Point(454, 424);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(233, 86);
            this.btnConfirm.TabIndex = 8;
            this.btnConfirm.Text = "Confirm Appointment";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "MM/dd/yyyy";
            this.dtpDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(64, 289);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 35);
            this.dtpDate.TabIndex = 9;
            this.dtpDate.Value = new System.DateTime(2023, 11, 6, 0, 0, 0, 0);
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtp_ValueChanged);
            // 
            // lvwAppointment
            // 
            this.lvwAppointment.HideSelection = false;
            this.lvwAppointment.Location = new System.Drawing.Point(419, 114);
            this.lvwAppointment.Name = "lvwAppointment";
            this.lvwAppointment.Size = new System.Drawing.Size(300, 293);
            this.lvwAppointment.TabIndex = 10;
            this.lvwAppointment.UseCompatibleStateImageBehavior = false;
            // 
            // AppointmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 564);
            this.Controls.Add(this.lvwAppointment);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.lblQuestion3);
            this.Controls.Add(this.lblQuestion2);
            this.Controls.Add(this.cbxServiceSelection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblNewAppointment);
            this.Name = "AppointmentForm";
            this.Text = "AppointmentForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNewAppointment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxServiceSelection;
        private System.Windows.Forms.Label lblQuestion2;
        private System.Windows.Forms.Label lblQuestion3;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ListView lvwAppointment;
    }
}