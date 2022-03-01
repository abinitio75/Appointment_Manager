using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
namespace Appointment_Manager
{
    public partial class ReportsScreen : Form
    {
        public ReportsScreen()
        {
            InitializeComponent();
            Height = 290;
            Width = 130;
            dgvReport.Height = 200;
            dgvReport.Width = 110;
            dgvReport.Left = 5;
            btnOkay.Top = 220;
            btnOkay.Left = 35;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Prime Time Report";
            PrimeTimeReport();
        }
        
        public ReportsScreen(string s)
        {
            InitializeComponent();
            Height = 400;
            Width = 380;
            dgvReport.Height = 300;
            dgvReport.Width = 355;
            dgvReport.Left = 5;
            btnOkay.Top = 325;
            btnOkay.Left = 155;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Schedule Report";
            ScheduleReport();
        }

        public ReportsScreen(int i)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Appointment Report";
            Height = 400;
            Width = 220;
            dgvReport.Width = 180;
            dgvReport.Height = 300;
            dgvReport.Left = 10;
            btnOkay.Top = 325;
            btnOkay.Left = 75;
            AppointmentReport();
        }

        private void AppointmentReport()
        {
            SortedDictionary<string, SortedDictionary<string, int>> apptReport = new SortedDictionary<string, SortedDictionary<string, int>>();
            
            DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime end = start.AddMonths(1);
            Report report = new Report();
            
            try
            {
                do
                {
                    report.GetAppointmentsReport(start, end);
                    apptReport.Add(
                        System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(start.Month),
                        report.apptReport);

                    start = start.AddMonths(1);
                    end = start.AddMonths(1);
                }
                while (start.Month != 1);
            }
            catch(Exception e)
            {
                Common.WriteToLog(e.Message);
                MessageBox.Show(e.Message);
            }

            dgvReport.DataSource = apptReport.SelectMany(dict => dict.Value, (dict, val) => new
            {
                Month = dict.Key.ToString(),
                Type = val.Key.ToString(),
                Count = val.Value.ToString()
            }).ToList();

        }

        private void ScheduleReport()
        {
            Report report = new Report();
            report.GetScheduleReport();
            dgvReport.DataSource = report.scheduleReport;
        }

        private void PrimeTimeReport()
        {
            Report report = new Report();
            report.GetPrimeTimeReport();
            dgvReport.DataSource = report.primeTimeReport;
        }
        private void btnOkay_Click(object sender, EventArgs e) => Close();
    }
}
