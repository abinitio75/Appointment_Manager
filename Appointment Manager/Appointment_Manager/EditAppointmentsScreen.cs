using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Appointment_Manager
{
    public partial class EditAppointmentsScreen : Form
    {
        private Appointment appointment = new Appointment();
        private readonly List<Customer> customerList;
        private readonly SortedList<DateTime, Appointment> appointmentList;

        public EditAppointmentsScreen(ref List<Customer> customerList, ref SortedList<DateTime, Appointment> appointmentList)
        {
            InitializeComponent();
            Text = "Add Appointment";
            StartPosition = FormStartPosition.CenterScreen;
            this.customerList = customerList;
            this.appointmentList = appointmentList;
            SetControls();
            EnableControls(false);
            SetInputLimits();
            btnAddAppointment.Text = "Add";
        }
        
        public EditAppointmentsScreen(Appointment appointment, ref List<Customer> customerList, ref SortedList<DateTime, Appointment> appointmentList)
        {
            InitializeComponent();
            Text = "Edit Appointment";
            StartPosition = FormStartPosition.CenterScreen;
            this.customerList = customerList;
            this.appointment = appointment;
            this.appointmentList = appointmentList;
            SetControls();
            SetInputLimits();
            btnAddAppointment.Text = "Update";
        }

        private void SetControls()
        {
            dtpStart.CustomFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern +
                " " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            dtpEnd.CustomFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern +
                " " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            listCustomerSelect.DataSource = customerList;
            listCustomerSelect.DisplayMember = "customerName";
            
            if (!string.IsNullOrEmpty(appointment.CustomerName))
            {
                listCustomerSelect.SelectedItem = customerList.Where(c => c.CustomerID == appointment.CustomerID).Single();
                listCustomerSelect.Enabled = false;
                btnCustomerSelect.Enabled = false;
                EnableControls(true);
                txtContact.Text = appointment.Contact;
                txtDescription.Text = appointment.Description;
                txtLocation.Text = appointment.Location;
                txtTitle.Text = appointment.Title;
                txtType.Text = appointment.Type;
            }
        }

        private void SetInputLimits()
        {
            txtContact.MaxLength = 255;
            txtDescription.MaxLength = 255;
            txtLocation.MaxLength = 255;
            txtTitle.MaxLength = 255;
            txtType.MaxLength = 255;
        }

        private void EditAppointments_Load(object sender, EventArgs e)
        {
            //empty
        }
        
        private void btnCustomerSelect_Click(object sender, EventArgs e)
        {   
            if(listCustomerSelect.SelectedItems.Count > 0)
                EnableControls(true);
            else
                MessageBox.Show("Please make a selection.");
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {   
            DateTime apptStart = dtpStart.Value;
            DateTime apptEnd = dtpEnd.Value;

            bool CheckTimeIsInvalid(ref DateTime start, ref DateTime end, DateTime startTime, DateTime endTime) =>
                start.Hour < startTime.Hour || start.Hour > endTime.Hour || end.Hour > endTime.Hour || end.Hour < start.Hour;

            if (Controls.OfType<TextBox>().Select(s => s.Text is null || s.Text.Length < 1).DefaultIfEmpty(false).FirstOrDefault())
            {
                MessageBox.Show("Please fill out all fields in the form");
            }
            else if(CheckTimeIsInvalid(ref apptStart, ref apptEnd, DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)))
            {
                MessageBox.Show("Ensure the times chosen are within normal operating hours");
            }
            else
            {
                Func<DateTime, DateTime, SortedList<DateTime, Appointment>, int> getOverlappingAppointment = (dtStart, dtEnd, apptList) =>
                    apptList.AsEnumerable().Where(appt => appt.Value.Start >= dtStart && appt.Value.Start <= dtEnd ||
                    appt.Value.End >= dtStart && appt.Value.End <= dtEnd).Select(appt => appt.Value.AppointmentID).
                    DefaultIfEmpty(0).FirstOrDefault();
                
                if (getOverlappingAppointment(apptStart, apptEnd, appointmentList) > 0)
                {
                    MessageBox.Show("Overlapping appointments detected");
                }
                else
                {
                    appointment.Title = txtTitle.Text;
                    appointment.Description = txtDescription.Text;
                    appointment.Type = txtType.Text;
                    appointment.Location = txtLocation.Text;
                    appointment.Contact = txtContact.Text;
                    appointment.Start = apptStart;
                    appointment.End = apptEnd;

                    if (string.IsNullOrEmpty(appointment.CustomerName))
                    {
                        appointment.CustomerID = ((Customer)listCustomerSelect.SelectedItem).CustomerID;
                        appointment.Add();
                    }
                    else
                    {
                        appointment.Update();
                    }
                    Close();
                }
            }
        }

        private void EnableControls(bool enable)
        {
            foreach (Control control in this.Controls)
            {
                if (control != listCustomerSelect && control != btnCustomerSelect)
                {
                    control.Visible = enable;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
