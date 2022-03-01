using System;
using System.Collections.Generic;
using System.Data;

namespace Appointment_Manager
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool Active { get; set; }
        public int AddressID { get; set; }
        public string Address { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public int CountryID { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }

        public Customer() { }

        public Customer(string customerName, int customerID, bool active, string address, string address2, int addressID, string city, int cityID, string country,
            int countryID, string phone, string postalCode)
        {
            CustomerID = customerID;
            CustomerName = customerName;
            Active = active;
            AddressID = addressID;
            Address = address;
            CityID = cityID;
            City = city;
            CountryID = countryID;
            Country = country;
            Phone = phone;
            Address2 = address2;
            PostalCode = postalCode;
        }

        public override string ToString()
        {
            return CustomerID.ToString() + " " +
            CustomerName.ToString() + " " +
            Active.ToString() + " " +
            AddressID.ToString() + " " +
            Address.ToString() + " " +
            CityID.ToString() + " " +
            City.ToString() + " " +
            CountryID.ToString() + " " +
            Country.ToString() + " " +
            Phone.ToString() + " " +
            Address2.ToString() + " " +
            PostalCode.ToString();
        }

        public void Add()
        {
            string now = Common.ConvertTimeFormat(DateTime.UtcNow);
            DBConnection db = new DBConnection();

            try
            {
                if (CheckCountryCity(ref now, db))
                {
                    AddressID = AddAddress(ref now, db);
                    AddCustomer(ref now, db);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                Common.WriteToLog(e.Message);
            }
        }

        public void Update()
        {
            string now = Common.ConvertTimeFormat(DateTime.UtcNow);
            DBConnection db = new DBConnection();

            try
            {
                if (CheckCountryCity(ref now, db))
                {
                    UpdateAddress(ref now, db);
                    UpdateCustomer(ref now, db);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                Common.WriteToLog(e.Message);
            }
        }

        private bool CheckCountryCity(ref string now, DBConnection db)
        {
            try
            {
                if (CountryID == 0)
                {
                    CountryID = AddCountry(ref now, db);
                    if (CountryID < 0)
                        throw new Exception("Failed to add country to database");
                }
                if (CityID == 0)
                {
                    CityID = AddCity(ref now, db);
                    if (CityID < 0)
                        throw new Exception("Failed to add city to database");
                }
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                Common.WriteToLog(e.Message);
                return false;
            }
        }

        private int AddCountry(ref string now, DBConnection db)
        {
            string sqlString = "SELECT countryId FROM country WHERE country LIKE @countryName;";
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@countryName", Country);
            object countryID = db.GetScalar();
            db.SqlCommand.Parameters.Clear();
            if (!Equals(countryID, null))
            {
                return Convert.ToInt32(countryID);
            }
            else
            {
                sqlString = "INSERT INTO country (country, createDate, createdBy, lastUpdateBy)" +
                   " VALUES(@country, @now, @createdBy, @lastUpdateBy);";
                db.SqlCommand.CommandText = sqlString;
                db.SqlCommand.Parameters.AddWithValue("@country", Country);
                db.SqlCommand.Parameters.AddWithValue("@now", now);
                db.SqlCommand.Parameters.AddWithValue("@createdBy", User.UserName);
                db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
                db.ExecuteCommand();
                db.SqlCommand.Parameters.Clear();
                return db.GetLastInsertID();
            }
        }

        private int AddCity(ref string now, DBConnection db)
        {
            string sqlString = "SELECT cityId FROM city WHERE city LIKE @cityName;";
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@cityName", City);
            object cityID = db.GetScalar();
            db.SqlCommand.Parameters.Clear();
            if (!Equals(cityID, null))
            {
                return Convert.ToInt32(cityID);
            }
            else
            {
                sqlString = "INSERT INTO city(city, countryId, createDate, createdBy, lastUpdateBy)" +
                    " VALUES(@city, @countryID, @now, @createdBy, @lastUpdateBy);";
                db.SqlCommand.CommandText = sqlString;
                db.SqlCommand.Parameters.AddWithValue("@city", City);
                db.SqlCommand.Parameters.AddWithValue("@countryID", CountryID);
                db.SqlCommand.Parameters.AddWithValue("@now", now);
                db.SqlCommand.Parameters.AddWithValue("@createdBy", User.UserName);
                db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
                db.ExecuteCommand();
                db.SqlCommand.Parameters.Clear();
                return db.GetLastInsertID();
            }
        }

        private int AddAddress(ref string now, DBConnection db)
        {
            string sqlString = "INSERT INTO address(address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdateBy)" +
                    " VALUES(@address, @address2, @cityID, @postalCode, @phone, @now, @createdBy, @lastUpdateBy);";
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@address", Address);
            db.SqlCommand.Parameters.AddWithValue("@address2", Address2);
            db.SqlCommand.Parameters.AddWithValue("@cityID", CityID);
            db.SqlCommand.Parameters.AddWithValue("@postalCode", PostalCode);
            db.SqlCommand.Parameters.AddWithValue("@phone", Phone);
            db.SqlCommand.Parameters.AddWithValue("@now", now);
            db.SqlCommand.Parameters.AddWithValue("@createdBy", User.UserName);
            db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
            db.ExecuteCommand();
            db.SqlCommand.Parameters.Clear();
            return db.GetLastInsertID();
        }

        private void UpdateAddress(ref string now, DBConnection db)
        {
            string sqlString = "UPDATE address SET address = @address, address2 = @address2, cityId = @cityID, postalCode = @postalCode," +
                " phone = @phone, lastUpdate = @now, lastUpdateBy = @lastUpdateBy WHERE addressId = @addressID;";
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@address", Address);
            db.SqlCommand.Parameters.AddWithValue("@address2", Address2);
            db.SqlCommand.Parameters.AddWithValue("@cityID", CityID);
            db.SqlCommand.Parameters.AddWithValue("@postalCode", PostalCode);
            db.SqlCommand.Parameters.AddWithValue("@phone", Phone);
            db.SqlCommand.Parameters.AddWithValue("@now", now);
            db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
            db.SqlCommand.Parameters.AddWithValue("@addressID", AddressID);
            db.ExecuteCommand();
            db.SqlCommand.Parameters.Clear();
        }

        private void AddCustomer(ref string now, DBConnection db)
        {
            string sqlString = "INSERT INTO customer(customerName, addressId, active, createDate, createdBy, lastUpdateBy)" +
                    " VALUES(@customerName, @addressID, @active, @now, @createdBy, @lastUpdateBy);";
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@customerName", CustomerName);
            db.SqlCommand.Parameters.AddWithValue("@addressID", AddressID);
            db.SqlCommand.Parameters.AddWithValue("@active", Active);
            db.SqlCommand.Parameters.AddWithValue("@now", now);
            db.SqlCommand.Parameters.AddWithValue("@createdBy", User.UserName);
            db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
            db.ExecuteCommand();
        }

        private void UpdateCustomer(ref string now, DBConnection db)
        {
            string sqlString = "UPDATE customer SET customerName = @customerName, active = @active, lastUpdate = @now," +
                " lastUpdateBy = @lastUpdateBy WHERE customer.customerId = @customerID";
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@customerName", CustomerName);
            db.SqlCommand.Parameters.AddWithValue("@active", Active);
            db.SqlCommand.Parameters.AddWithValue("@now", now);
            db.SqlCommand.Parameters.AddWithValue("@lastUpdateBy", User.UserName);
            db.SqlCommand.Parameters.AddWithValue("@customerID", CustomerID);
            db.ExecuteCommand();
        }

        public void DeleteCustomer()
        {
            string sqlString = "DELETE FROM customer WHERE customerId = @customerID;";
            DBConnection db = new DBConnection();
            db.SqlCommand.CommandText = sqlString;
            db.SqlCommand.Parameters.AddWithValue("@customerID", CustomerID);
            db.ExecuteCommand();
        }
    }
}
