using System;
using System.Data;

namespace Appointment_Manager
{
    public static class User
    {
        private static int userID;
        private static string userName;
        public static int UserID => userID;
        public static string UserName => userName;

        public static void SetUserID(int id) => userID = id;
        public static void SetUserName(string name) => userName = name;

        public static string GetHash(string str) => BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().ComputeHash
                (System.Text.Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();

        public static bool Login(string userName, string pwd)
        {
            string sqlString = "SELECT EXISTS(SELECT userName, password FROM user WHERE userName = @userName and password = @password);";
            string hashPwd = GetHash(pwd);
            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@userName", userName);
            db.SqlCommand.Parameters.AddWithValue("@password", hashPwd);
            object obj = db.GetScalar();
            if (!Equals(obj, null))
            {
                return Convert.ToBoolean(obj);
            }
            else
            {
                Common.WriteToLog("Login attempt failed for user: " + userName);
                return false;
            }
        }

        public static bool Login(string userName, string pwd, string connectionString)
        {
            string sqlString = "SELECT EXISTS(SELECT userName, password FROM user WHERE userName = @userName and password = @password);";
            string hashPwd = GetHash(pwd);
            DBConnection db = new DBConnection(connectionString);
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@userName", userName);
            db.SqlCommand.Parameters.AddWithValue("@password", hashPwd);
            object obj = db.GetScalar();
            return Convert.ToBoolean(obj);
        }

        public static void SetCurrentUser(string userName)
        {
            string sqlString = "SELECT userName, userId FROM user WHERE userName = @userName;";

            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@userName", userName);
            DataRow dr = db.GetDataRow();
            if (dr.Table.Rows.Count != 0)
            {
                SetUserID(dr.Field<int>("userId"));
                SetUserName(dr.Field<string>("userName"));
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There was a problem setting the current user. Please logout and log back in to prevent unnecessary application behavior.");
            }
        }
    }
}
