using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Appointment_Manager
{
    public class Common
    {
        public static void WriteToLog(bool login)
        {
            if (!File.Exists("Log.txt"))
            {
                File.Create("Log.txt");
            }
            File.AppendAllText("Log.txt", " - "
            + (login ? "Login" : "Logout") + " by \"" + User.UserName + "\", UserID = "
            + User.UserID + " @ " + DateTime.Now + "\n");
        }

        public static void WriteToLog(string message)
        {
            if (!File.Exists("ErrorLog.txt"))
            {
                File.Create("ErrorLog.txt");
            }
            File.AppendAllText("ErrorLog.txt", $"{message}" + " : " + DateTime.Now + "\n");
        }

        public static string ConvertTimeFormat(DateTime dt) => dt.ToString("yyyy-MM-dd HH:mm:ss");

        public static List<Customer> GetCustomerList()
        {
            List<Customer> customerList = new List<Customer>();

            string sqlString = "SELECT c.customerId, c.customerName, c.active, c.addressId, a.address, a.address2, a.postalCode, a.phone, a.cityId, ci.city, ci.countryId, co.country" +
                " FROM customer AS c INNER JOIN address AS a ON c.addressId = a.addressId INNER JOIN city AS ci ON a.cityId = ci.cityId" +
                " INNER JOIN country AS co ON ci.countryId = co.countryId;";
            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            DataTable dt = db.GetDataTable();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    customerList.Add(new Customer(
                        customerName: dr.Field<string>("customerName"), customerID: dr.Field<int>("customerId"), active: dr.Field<bool>("active"),
                        address: dr.Field<string>("address"), address2: dr.Field<string>("address2"), addressID: dr.Field<int>("addressId"),
                        city: dr.Field<string>("city"), cityID: dr.Field<int>("cityId"), country: dr.Field<string>("country"),
                        countryID: dr.Field<int>("countryId"), phone: dr.Field<string>("phone"), postalCode: dr.Field<string>("postalCode")));
                }
                customerList.Sort(new Comparison<Customer>((x, y) => string.Compare(x.CustomerName, y.CustomerName)));
            }
            return customerList;
        }

        public static SortedList<DateTime, Appointment> GetAppointmentsList(bool month)
        {
            SortedList<DateTime, Appointment> appointmentList = new SortedList<DateTime, Appointment>();

            string dtBegin = ConvertTimeFormat(DateTime.UtcNow);
            string dtEnd = month ? ConvertTimeFormat(DateTime.UtcNow.AddMonths(1)) : Common.ConvertTimeFormat(DateTime.UtcNow.AddDays(7));
            string sqlString = "SELECT start, end, customer.customerName, title, description, type, location, contact, url, appointmentId, appointment.customerId" +
                " FROM appointment INNER JOIN customer ON customer.customerId = appointment.customerId AND appointment.start" +
                " BETWEEN @dtBegin AND @dtEnd WHERE EXISTS(SELECT appointmentId FROM appointment WHERE appointment.userID = @User.UserID);";
            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@dtBegin", dtBegin);
            db.SqlCommand.Parameters.AddWithValue("@dtEnd", dtEnd);
            db.SqlCommand.Parameters.AddWithValue("@User.UserID", User.UserID);
            DataTable dt = db.GetDataTable();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    appointmentList.Add(Convert.ToDateTime(dr.Field<DateTime>("start")).ToLocalTime(),
                        new Appointment(customerName: dr.Field<string>("customerName"), start: Convert.ToDateTime(dr.Field<DateTime>("start")).ToLocalTime(),
                        end: Convert.ToDateTime(dr.Field<DateTime>("end")).ToLocalTime(), type: dr.Field<string>("type"),
                        title: dr.Field<string>("title"), description: dr.Field<string>("description"), location: dr.Field<string>("location"),
                        contact: dr.Field<string>("contact"), appointmentID: dr.Field<int>("appointmentId"), url: dr.Field<string>("url"),
                        customerID: dr.Field<int>("customerId")));
                }
            }
            return appointmentList;
        }
    }
}