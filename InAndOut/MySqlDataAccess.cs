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

            io.WriteToFile(Filename, dr);
        }

        public void SaveAction(string sqlCommand, int userId, Enums.Actions action, string currentTime)
        {
            MySqlCommand command2 = new MySqlCommand();
            command2.Connection = conn;
            command2.CommandType = CommandType.Text;
            command2.CommandText = sqlCommand;

            conn.Open();
            //conn.Open();

            string query = sqlCommand;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
            DateTime dt = DateTime.ParseExact(currentTime, ConfigurationManager.AppSettings["DateTimeFormat"], CultureInfo.CurrentCulture);
           
            command2.Parameters.AddWithValue("@userId", userId);
            command2.Parameters.AddWithValue("@fecha", dt.ToShortDateString());
            command2.Parameters.AddWithValue("@hora", dt.ToString("HH:mm"));
            command2.Parameters.AddWithValue("@actionId", action);

            // ... other parameters
            command2.ExecuteNonQuery();
            
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

        public void InsertDataIntoMySQLServerUsingBulkCopy(DataTable csvFileData)
        {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[2].ToString()))
            {
                conn.Open();

                String sql = null;
                String sqlStart = "INSERT into TempConsolidatedReport Values ";
                int x = 0;


                var newDataTable = csvFileData.AsEnumerable()
               .OrderBy(r => r.Field<string>("Usuario"))
               .ThenBy(r => r.Field<string>("fecha"))
               .ThenBy(r => r.Field<string>("hora"))
               .CopyToDataTable();

                foreach (DataRow row in newDataTable.Rows)
                {
                    x += 1;
                    if (x == 1)
                    {
                        sql = String.Format(@"({0},{1},{2},{3})",
                                              row["Usuario"],
                                              row["fecha"],
                                              row["hora"],
                                              row["Evento"]
                                              );
                    }
                    else
                    {
                        sql = String.Format(sql + @",({0},{1},{2},{3})",
                                              row["Usuario"],
                                              row["fecha"],
                                              row["hora"],
                                              row["Evento"]
                                              );

                    }

                    if (x == 1000)
                    {
                        try
                        {
                            sql = sqlStart + sql;
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Write {0}", x);
                            x = 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(sql);
                            Console.WriteLine(ex.ToString());
                        }
                    }

                }
                // get any straglers
                if (x > 0)
                {
                    try
                    {
                        sql = sqlStart + sql;
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Write {0}", x);
                        x = 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(sql);
                        Console.WriteLine(ex.ToString());
                    }

                }

                conn.Close();
            }
        }
    }
}
