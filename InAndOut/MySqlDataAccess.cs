using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading;

namespace InAndOut
{
    public class MySqlDataAccess
    {

        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[2].ToString());
        int lastActivity;

        /// <summary>
        /// Method for testing connectivity
        /// </summary>
        /// <returns></returns>
        //public int connectMySql()
        //{
        //    mySqlConn.Open();
        //    if (mySqlConn.State == ConnectionState.Open)
        //    {
        //        MySqlCommand command2 = new MySqlCommand();
        //        command2.Connection = mySqlConn;
        //        command2.CommandType = CommandType.Text;
        //        command2.CommandText = "Select * From Users";

        //        MySqlDataReader reader = command2.ExecuteReader();
        //        //SqlDataReader reader = command.ExecuteReader();
        //        reader.Read();
        //        if (reader.HasRows)
        //            return reader.GetInt32(0);
        //        else
        //            return 0;
        //        //conn.Close();
        //        mySqlConn.Close();                
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        public int ReadLastActivity(string sqlCommand)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;

            conn.Open();
            //conn.Open();

            // Read data returned for the query.
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                //SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                    lastActivity = reader.GetInt32(0);
                else
                    lastActivity = 0;
                //conn.Close();
                conn.Close();

                return lastActivity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return 0;
            }
        }

        public bool GetUser(string sqlCommand, string user, byte[] password)
        {

            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            
            // Specify the query to be executed.
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;
            // Open a connection to database.
            command.Parameters.AddWithValue("@userName", user);
            command.Parameters.AddWithValue("@password", password);
            conn.Open();

            // Read data returned for the query.
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Global.appUser = (string)reader["userName"];
                    Global.appUserId = (int)reader["id"];
                }
                reader.Close();
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        public void GetRecords(string storedProcedure, string Filename, string date1, string date2, int userId = 0)
        {
            conn.Open();
            MySqlCommand command = new MySqlCommand(storedProcedure, conn);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
            var dateFrom = DateTime.ParseExact(date1, ConfigurationManager.AppSettings["DateFormat"], CultureInfo.InvariantCulture);
            var dateTo = DateTime.ParseExact(date2, ConfigurationManager.AppSettings["DateFormat"], CultureInfo.InvariantCulture);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@dateFrom", dateFrom);
            command.Parameters.AddWithValue("@dateTo", dateTo);
            MySqlDataReader dr = command.ExecuteReader();

            IO io = new IO();
            ExcelUtility eu = new ExcelUtility();

            var dataTable = new DataTable();
            dataTable.Load(dr);

            eu.WriteDataTableToExcel(dataTable,"Prueba","C:\\prueba.xlsx","Test");
            
            //io.WriteToFile(Filename, dr);
        }

        public void SaveAction(string sqlCommand, int userId, Enums.Actions action, string currentTime, string observaciones, string station)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;

            

            string query = sqlCommand;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
            DateTime dt = DateTime.ParseExact(currentTime, ConfigurationManager.AppSettings["DateTimeFormat"], CultureInfo.CurrentCulture);
           
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@fecha", dt.ToShortDateString());
            command.Parameters.AddWithValue("@hora", dt.ToString("HH:mm"));
            command.Parameters.AddWithValue("@actionId", action);
            command.Parameters.AddWithValue("@station", station);
            command.Parameters.AddWithValue("@observaciones", observaciones);
            conn.Open();
            //conn.Open();
            // ... other parameters
            command.ExecuteNonQuery();
            
            conn.Close();
        }

        public void RegisterUser(string sqlCommand, string user, byte[] password)
        {

            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;

            // Specify the query to be executed.
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;
            // Open a connection to database.

            conn.Open();

            string query = sqlCommand;

            command.Parameters.AddWithValue("@userName", user);
            command.Parameters.AddWithValue("@password", password);

            // ... other parameters
            command.ExecuteNonQuery();

            conn.Close();
        }        
    }
}
