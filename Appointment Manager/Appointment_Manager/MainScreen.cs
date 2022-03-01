using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Timers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appointment_Manager
{
    public partial class MainScreen : Form
    {
        private static SortedList<DateTime, Appointment> appointmentList = new SortedList<DateTime, Appointment>();
        private List<Customer> customerList = new List<Customer>();

        private readonly System.Timers.Timer timer = new System.Timers.Timer(300000);
        private bool empty;

        public MainScreen()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        
        private void MainScreen_Load(object sender, EventArgs e)
        {
            customerList = Common.GetCustomerList();
            tsBtnMonthView_Click(tsBtnMonthView, new EventArgs());
            StartTimer();
            System.Timers.Timer t = new System.Timers.Timer(1500);
            t.Elapsed += CheckAppointmentInterval;
            t.Enabled = true;
            t.AutoReset = false;
        }

        private void StartTimer()
        {
            timer.Elapsed += CheckAppointmentInterval;
            timer.Enabled = true;
            GC.KeepAlive(timer);
            timer.Start();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!empty)
            {
                if (tsBtnCustomerList.Checked)
                {
                    new EditCustomersScreen(ref customerList).ShowDialog();
                    tsBtnCustomerList_Click(tsBtnCustomerList, new EventArgs());
                }
                else
                {
                    new EditAppointmentsScreen(ref customerList, ref appointmentList).ShowDialog();

                    if (tsBtnMonthView.Checked)
                        tsBtnMonthView_Click(tsBtnMonthView, new EventArgs());
                    else
                        tsBtnWeekView_Click(tsBtnWeekView, new EventArgs());
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!empty)
            {
                if (dgvView.CurrentRow.Selected)
                {
                    if (tsBtnCustomerList.Checked)
                    {
                        new EditCustomersScreen(ref customerList, (Customer)dgvView.CurrentRow.DataBoundItem).ShowDialog();
                        tsBtnCustomerList_Click(tsBtnCustomerList, new EventArgs());
                    }
                    else
                    {
                        new EditAppointmentsScreen((Appointment)dgvView.CurrentRow.DataBoundItem, ref customerList, ref appointmentList).ShowDialog();

                        if (tsBtnMonthView.Checked)
                            tsBtnMonthView_Click(tsBtnMonthView, new EventArgs());
                        else
                            tsBtnWeekView_Click(tsBtnWeekView, new EventArgs());
                    }
                }
                else
                    MessageBox.Show("Please make a selection first.");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {   
            if (dgvView.CurrentRow.Selected)
            {
                if (!empty)
                {
                    if (tsBtnCustomerList.Checked)
                    {
                        DialogResult result = MessageBox.Show("Really delete " + (string)dgvView.CurrentRow.Cells["customerName"].Value + "?", "Alert!", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            ((Customer)dgvView.CurrentRow.DataBoundItem).DeleteCustomer();
                            tsBtnCustomerList_Click(tsBtnCustomerList, new EventArgs());
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Really delete appointment with " + (string)dgvView.CurrentRow.Cells["customerName"].Value + "?", "Alert!", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            ((Appointment)dgvView.CurrentRow.DataBoundItem).DeleteAppointment();
                            if (tsBtnMonthView.Checked)
                                tsBtnMonthView_Click(tsBtnMonthView, new EventArgs());
                            else
                                tsBtnWeekView_Click(tsBtnWeekView, new EventArgs());
                        }
                    }
                }
            }
        }
        
        private void tsBtnMonthView_Click(object sender, EventArgs e)
        {
            UncheckButtons(ref sender);
            SetView();
            tsLabelAppointment.Text = "Month Appointments";
        }
        
        private void tsBtnWeekView_Click(object sender, EventArgs e)
        {
            UncheckButtons(ref sender);
            SetView();
            tsLabelAppointment.Text = "Week Appointments";
        }

        private void tsBtnCustomerList_Click(object sender, EventArgs e)
        {   
            UncheckButtons(ref sender);
            SetView();
        }
        
        private void SetView()
        {
            if (!tsBtnCustomerList.Checked)
            {
                Text = "Appointments";
                appointmentList = Common.GetAppointmentsList(tsBtnMonthView.Checked);
                if (appointmentList.Count > 0)
                {
                    dgvView.DataSource = appointmentList.Select(vals => vals.Value).ToList();
                    dgvView.Columns["start"].DisplayIndex = 0;
                    dgvView.Columns["end"].DisplayIndex = 1;
                    dgvView.Columns["customerName"].DisplayIndex = 2;
                    dgvView.Columns["type"].DisplayIndex = 3;
                    dgvView.Columns["description"].DisplayIndex = 4;
                    empty = false;
                }
                else
                {
                    empty = true;
                    dgvView.DataSource = new List<string>() { $"No appointments are scheduled this {(tsBtnMonthView.Checked ? "month" : "week")}." }.Select(s => new { result = s }).ToList();
                }
            }
            else
            {
                Text = "Customers";
                customerList = Common.GetCustomerList();
                if (customerList.Count > 0)
                {
                    dgvView.DataSource = customerList;
                    dgvView.Columns["customerName"].DisplayIndex = 0;
                    dgvView.Columns["phone"].DisplayIndex = 1;
                    dgvView.Columns["address"].DisplayIndex = 2;
                    dgvView.Columns["address2"].DisplayIndex = 3;
                    dgvView.Columns["postalCode"].DisplayIndex = 4;
                    dgvView.Columns["city"].DisplayIndex = 5;
                    dgvView.Columns["country"].DisplayIndex = 6;
                    tsLabelAppointment.Text = "Customer List";
                    empty = false;
                }
                else
                {
                    empty = true;
                    dgvView.DataSource = new List<string>() { "There are no customers at this time." }.Select(s => new { result = s }).ToList();
                }
            }
        }
        
        private void UncheckButtons(ref object sender)
        {
            foreach (ToolStripButton btn in toolStrip1.Items.OfType<ToolStripButton>())
            {
                btn.Checked = sender == btn;
            }
        }

        private void CheckAppointmentInterval(object caller, ElapsedEventArgs e)
        {
            if(appointmentList.Count > 0)
            {
                Func<SortedList<DateTime, Appointment>, DateTime, DateTime, string> checkForUpcomingAppointment = (apptList, dtNow, dtRange) =>
                   apptList.Values.AsEnumerable().Where(appt => appt.Start > dtNow &&
                   appt.Start < dtRange).Select(appt => appt.CustomerName).DefaultIfEmpty("").First();

                string appointmentName;
                DateTime now = DateTime.Now;
                DateTime range = now.AddMinutes(15);
                
                appointmentName = checkForUpcomingAppointment(appointmentList, now, range);
                
                if (!string.IsNullOrEmpty(appointmentName))
                {
                    DateTime appt = appointmentList.Where(appointment => appointment.Value.CustomerName ==
                        appointmentName).Select(kvp => kvp.Key).FirstOrDefault();
                    
                    MessageBox.Show("You have an upcoming appointment with " + appointmentName + " at " + 
                        appt.ToString("t"));
                }
            }
        }
        
        private void btnAppointmentTypeReport_Click(object sender, EventArgs e)
        {
            if (!empty)
            {
                Hide();
                new ReportsScreen(0).ShowDialog();
                Show();
            }
        }

        private void btnScheduleReport_Click(object sender, EventArgs e)
        {
            if (!empty)
            {
                Hide();
                new ReportsScreen("").ShowDialog();
                Show();
            }
        }

        private void btnPeakTimes_Click(object sender, EventArgs e)
        {
            if (!empty)
            {
                Hide();
                new ReportsScreen().ShowDialog();
                Show();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e) => Close();

        private void btnExit_Click(object sender, EventArgs e) => Application.Exit();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!empty)
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    if (tsBtnCustomerList.Checked)
                        dgvView.DataSource = SearchCustomers(txtSearch.Text, customerList);
                    else
                        dgvView.DataSource = SearchAppointments(txtSearch.Text, appointmentList);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!empty)
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    if (tsBtnCustomerList.Checked)
                        dgvView.DataSource = customerList;
                    else
                        dgvView.DataSource = appointmentList.Select(vals => vals.Value).ToList();
                }
            }
        }

        public List<Customer> SearchCustomers(string searchTerm, List<Customer> customers)
        {
            return customers.FindAll(
                c => c.ToString().ToLower().Contains(searchTerm.ToLower()));
        }

        public List<Appointment> SearchAppointments(string searchTerm, SortedList<DateTime, Appointment> appointments)
        {
            return appointments.Values.Where(
                a => a.ToString().ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }
    }
}
