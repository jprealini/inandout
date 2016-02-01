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

namespace InAndOut
{
    public class DataAccess
    {

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ToString());
        int lastActivity;

        public int ReadLastActivity(string sqlCommand)
        {

            SqlCommand command = new SqlCommand();
            command.Connection = conn;

            // Specify the query to be executed.
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;
            // Open a connection to database.

            conn.Open();

            // Read data returned for the query.
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                    lastActivity = reader.GetInt32(0);
                else
                    lastActivity = 0;
                conn.Close();

                return lastActivity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return 0;
            }
        }

        public bool GetUser(string sqlCommand, string user, string password)
        {

            SqlCommand command = new SqlCommand();
            command.Connection = conn;

            // Specify the query to be executed.
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;
            // Open a connection to database.
            command.Parameters.AddWithValue("@userName", user);
            command.Parameters.AddWithValue("@password", password);
            conn.Open();

            // Read data returned for the query.
            SqlDataReader reader = command.ExecuteReader();

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
            SqlCommand command = new SqlCommand(storedProcedure, conn);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@dateFrom", SqlDbType.DateTime).Value = DateTime.ParseExact(date1, ConfigurationManager.AppSettings["DateFormat"], CultureInfo.InvariantCulture);
            command.Parameters.Add("@dateTo", SqlDbType.DateTime).Value = DateTime.ParseExact(date2, ConfigurationManager.AppSettings["DateFormat"], CultureInfo.InvariantCulture);
            SqlDataReader dr = command.ExecuteReader();

            IO io = new IO();

            io.WriteToFile(Filename, dr);
        }

        public void SaveAction(string sqlCommand, int userId, Enums.Actions action, string currentTime)
        {

            SqlCommand command = new SqlCommand();
            command.Connection = conn;

            // Specify the query to be executed.
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;
            // Open a connection to database.
            conn.Open();

            string query = sqlCommand;

            DateTime dt = DateTime.ParseExact(currentTime, ConfigurationManager.AppSettings["DateTimeFormat"], CultureInfo.InvariantCulture);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@fecha", dt.Date);
            command.Parameters.AddWithValue("@hora", dt.TimeOfDay);
            command.Parameters.AddWithValue("@actionId", action);

            // ... other parameters
            command.ExecuteNonQuery();

            conn.Close();
        }

        public void RegisterUser(string sqlCommand, string user, string password)
        {

            SqlCommand command = new SqlCommand();
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

        public void InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData)
        {
            using (SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ToString()))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "TempConsolidatedReport";
                    foreach (var column in csvFileData.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    var newDataTable = csvFileData.AsEnumerable()
                   .OrderBy(r => r.Field<string>("Usuario"))
                   .ThenBy(r => r.Field<string>("fecha"))
                   .ThenBy(r => r.Field<string>("hora"))
                   .CopyToDataTable();
                    s.WriteToServer(newDataTable);
                }
                dbConnection.Close();
            }
        }        
    }
}
