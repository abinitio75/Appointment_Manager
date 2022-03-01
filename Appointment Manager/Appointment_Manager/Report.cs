using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Appointment_Manager
{
    public class Report
    {
        public List<object> scheduleReport;
        public SortedDictionary<string, int> apptReport;
        public List<object> primeTimeReport;
        private readonly DBConnection db = new DBConnection();

        public void GetScheduleReport()
        {
            scheduleReport = new List<object>();

            string sqlString = "SELECT start, end, user.userName, appointment.userId FROM appointment, user ORDER BY start, userId ASC";
            db.SqlCommand.CommandText = sqlString;
            DataTable dt = db.GetDataTable();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    scheduleReport.Add(new
                    {
                        Start = dr.Field<DateTime>("start").ToLocalTime(),
                        End = dr.Field<DateTime>("end").ToLocalTime(),
                        UserName = dr.Field<string>("userName"),
                        UserID = dr.Field<int>("userId")
                    });
                }
            }
        }

        public void GetAppointmentsReport(DateTime start, DateTime end)
        {
            apptReport = new SortedDictionary<string, int>();
            string dtBegin = Common.ConvertTimeFormat(start);
            string dtEnd = Common.ConvertTimeFormat(end);
            string sqlString = "SELECT type, COUNT(type) FROM appointment WHERE start BETWEEN @dtBegin AND @dtEnd GROUP BY type;";

            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@dtBegin", dtBegin);
            db.SqlCommand.Parameters.AddWithValue("@dtEnd", dtEnd);
            DataTable dt = db.GetDataTable();
            db.SqlCommand.Parameters.Clear();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    apptReport.Add(dr.Field<string>("type"), (int)dr.Field<long>("COUNT(type)"));
                }
            }
        }

        public void GetPrimeTimeReport()
        {
            primeTimeReport = new List<object>();

            string sqlString = "SELECT DISTINCT dayname(start) as DayName, AVG(extract(hour FROM start)) AS Hour FROM appointment GROUP BY DayName ORDER BY COUNT(*) DESC;";
            db.SqlCommand.CommandText = sqlString;
            DataTable dt = db.GetDataTable();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DateTime dateTime = new DateTime(DateTime.Now.Year, 1, 1, (int)dr.Field<decimal>("Hour"), 0, 0).ToLocalTime();
                    string hour = dateTime.ToString("t", System.Globalization.CultureInfo.CurrentCulture);

                    primeTimeReport.Add(new
                    {
                        Day = dr.Field<string>("DayName"),
                        AvgHour = hour
                    });
                }
            }
        }
    }
}
