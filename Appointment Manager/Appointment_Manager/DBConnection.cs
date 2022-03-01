using System;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Appointment_Manager
{
    public class DBConnection
    {
        private readonly MySqlConnection connection;
        private MySqlCommand sqlCommand;
        private MySqlDataReader reader;
        //public MySqlDataAdapter sqlDataAdapter;
        private int lastInsertID = -1;
        public MySqlCommand SqlCommand { get => sqlCommand; set => sqlCommand = value; }
        //public MySqlConnection Connection { get => connection; }
        public bool fail = false;

        public DBConnection()
        {
            connection = new MySqlConnection(
                ConfigurationManager.ConnectionStrings["ScheduleDBConnection"].ConnectionString);
            sqlCommand = new MySqlCommand("", connection);
        }

        public DBConnection(bool _)
        {
            connection = new MySqlConnection(
                ConfigurationManager.ConnectionStrings["EmptyDBConnection"].ConnectionString);
            sqlCommand = new MySqlCommand("", connection);
        }
        public DBConnection(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
            sqlCommand = new MySqlCommand("", connection);
        }

        public void DeleteDB()
        {
            string sqlString = "DROP DATABASE IF EXISTS scheduledb;";
            sqlCommand.CommandText = sqlString;

            try
            {
                connection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                fail = true;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool IsDBCreated()
        {
            string sqlString = "SELECT EXISTS(SELECT SCHEMA_NAME FROM information_schema.schemata WHERE SCHEMA_NAME = 'scheduledb');";
            sqlCommand.CommandText = sqlString;

            try
            {
                connection.Open();
                sqlCommand.Prepare();
                if (sqlCommand.ExecuteScalar().ToString() == "0")
                {
                    sqlCommand.CommandText = System.IO.File.ReadAllText("CreateDatabase.sql");
                    sqlCommand.Prepare();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "USE scheduledb;";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = System.IO.File.ReadAllText("CreateTables.sql");
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = System.IO.File.ReadAllText("CreateTableData.sql");
                    sqlCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ensure MySQL Server is running and that login credentials and database address are correct.\n" + e.Message);
                fail = true;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public int GetLastInsertID() => lastInsertID;

        public void ExecuteCommand()
        {
            try
            {
                connection.Open();
                sqlCommand.Prepare();
                sqlCommand.ExecuteNonQuery();
                lastInsertID = (int)sqlCommand.LastInsertedId;
            }
            catch (MySqlException dbEx)
            {
                Common.WriteToLog(dbEx.Message);
                MessageBox.Show("MySql Error: " + dbEx.Message);
                fail = true;
            }
            catch (Exception e)
            {
                Common.WriteToLog(e.Message);
                MessageBox.Show("Application Error: " + e.Message);
                fail = true;
            }
            finally
            {
                connection.Close();
            }
        }

        public DataRow GetDataRow()
        {
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                sqlCommand.Prepare();
                reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                    return dt.Rows[0];
                }
                return dt.NewRow();
            }
            catch (MySqlException dbEx)
            {
                Common.WriteToLog(dbEx.Message);
                MessageBox.Show("MySql Error: " + dbEx.Message);
                fail = true;
                return dt.NewRow();
            }
            catch (Exception e)
            {
                Common.WriteToLog(e.Message);
                MessageBox.Show("Application Error: " + e.Message);
                fail = true;
                return dt.NewRow();
            }
            finally
            {
                if (!reader.IsClosed)
                    reader.Close();
                connection.Close();
            }
        }

        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                sqlCommand.Prepare();
                reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                    return dt;
                }
                return new DataTable();
            }
            catch (MySqlException dbEx)
            {
                connection.Close();
                Common.WriteToLog(dbEx.Message);
                MessageBox.Show("MySql Error: " + dbEx.Message);
                fail = true;
                return new DataTable();
            }
            catch (Exception e)
            {
                connection.Close();
                Common.WriteToLog(e.Message);
                MessageBox.Show("Application Error: " + e.Message);
                fail = true;
                return new DataTable();
            }
            finally
            {
                if (!reader.IsClosed)
                    reader.Close();
                connection.Close();
            }
        }

        public object GetScalar()
        {
            object obj;

            try
            {
                connection.Open();
                sqlCommand.Prepare();
                obj = sqlCommand.ExecuteScalar();
                
                return obj;
            }
            catch (MySqlException dbEx)
            {
                Common.WriteToLog(dbEx.Message);
                MessageBox.Show("MySql Error: " + dbEx.Message);
                fail = true;
                return new object();
            }
            catch (Exception e)
            {
                Common.WriteToLog(e.Message);
                MessageBox.Show("Application Error: " + e.Message);
                fail = true;
                return new object();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
