using System;
using System.Collections.Generic;
using System.Data;

namespace Appointment_Manager
{
    public class Appointment
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string CustomerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type{ get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Url { get; set; }
        public int AppointmentID { get; set; }
        public int CustomerID { get; set; }
        
        public Appointment() { }

        public Appointment(string customerName, DateTime start, DateTime end, string type, string title, string description,
            string location, string contact, int appointmentID, string url, int customerID)
        {
            Start = start;
            End = end;
            CustomerName = customerName;
            Type = type;
            Title = title;
            Description = description;
            AppointmentID = appointmentID;
            Location = location;
            Contact = contact;
            Url = url;
            CustomerID = customerID;
        }

        public override string ToString()
        {
            return Start.ToString() + " " +
            End.ToString() + " " +
            CustomerName.ToString() + " " +
            Type.ToString() + " " +
            Title.ToString() + " " +
            Description.ToString() + " " +
            AppointmentID.ToString() + " " +
            Location.ToString() + " " +
            Contact.ToString() + " " +
            Url.ToString() + " " +
            CustomerID.ToString();
        }

        public void Add()
        {
            string aptStart = Common.ConvertTimeFormat(Start.ToUniversalTime());
            string aptEnd = Common.ConvertTimeFormat(End.ToUniversalTime());
            string now = Common.ConvertTimeFormat(DateTime.UtcNow);
            string sqlString = "INSERT INTO appointment (customerId, title, description, type, location, contact," +
                " url, start, end, createDate, userId, createdBy, lastUpdateBy) VALUES(@customerID, @title, @description," +
                " @type, @location, @contact, @customerID, @start, @end, @createDate, @userId , @createdBy, @lastUpdateBy);";
            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@customerID", CustomerID);
            db.SqlCommand.Parameters.AddWithValue("@title", Title);
            db.SqlCommand.Parameters.AddWithValue("@description", Description);
            db.SqlCommand.Parameters.AddWithValue("@type", Type);
            db.SqlCommand.Parameters.AddWithValue("@location", Location);
            db.SqlCommand.Parameters.AddWithValue("@contact", Contact);
            db.SqlCommand.Parameters.AddWithValue("@start", aptStart);
            db.SqlCommand.Parameters.AddWithValue("@end", aptEnd);
            db.SqlCommand.Parameters.AddWithValue("@createDate", now);
            db.SqlCommand.Parameters.AddWithValue("@userId", User.UserID);
            db.SqlCommand.Parameters.AddWithValue("@createdBy", User.UserName);
            db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
            db.ExecuteCommand();
        }

        public void Update()
        {
            string aptStart = Common.ConvertTimeFormat(Start.ToUniversalTime());
            string aptEnd = Common.ConvertTimeFormat(End.ToUniversalTime());
            string sqlString = "UPDATE appointment SET title = @title, description = @description," +
                "type = @type, location = @location, contact =@contact, url = @customerID, start = @start," +
                " end = @end, lastUpdateBy = @lastUpdateBy WHERE appointment.appointmentId = @appointmentID;";
            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@title", Title);
            db.SqlCommand.Parameters.AddWithValue("@description", Description);
            db.SqlCommand.Parameters.AddWithValue("@type", Type);
            db.SqlCommand.Parameters.AddWithValue("@location", Location);
            db.SqlCommand.Parameters.AddWithValue("@contact", Contact);
            db.SqlCommand.Parameters.AddWithValue("@customerID", CustomerID);
            db.SqlCommand.Parameters.AddWithValue("@start", aptStart);
            db.SqlCommand.Parameters.AddWithValue("@end", aptEnd);
            db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
            db.SqlCommand.Parameters.AddWithValue("@appointmentID", AppointmentID);
            db.ExecuteCommand();
        }

        public void DeleteAppointment()
        {
            string sqlString = "DELETE FROM appointment WHERE appointmentId = @appointmentID;";
            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@appointmentID", AppointmentID);
            db.ExecuteCommand();
        }
    }
}
