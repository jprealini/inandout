﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Windows.Forms;
using System.IO;

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
            bool status = false;
            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;

            // Specify the query to be executed.
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;
            // Open a connection to database.
            command.Parameters.AddWithValue("@userName", user);
            command.Parameters.AddWithValue("@password", password);
            try
            {
                conn.Open();

                // Read data returned for the query.
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Global.appUser = (string)reader["userName"];
                        Global.appUserId = (int)reader["id"];
                        Global.appUserIsAdmin = (bool)reader["isAdmin"];
                    }
                    reader.Close();
                    conn.Close();
                    status = true;
                    return status;
                }
                conn.Close();                
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido lograr una conexion al servidor de base de datos.\n\rVerifique su conexion a internet y vuelva a intentar.\n\rOriginal Error: '" + ex.Message + "'");
            }

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
            command.Parameters.AddWithValue("@dateFrom", date1);
            command.Parameters.AddWithValue("@dateTo", date2);
            var queryTest = CommandAsSql(command);
            MySqlDataReader dr = command.ExecuteReader();

            IO io = new IO();
            ExcelUtility eu = new ExcelUtility();

            var dataTable = new DataTable();
            dataTable.Load(dr);

            List<DataTable> result = dataTable.AsEnumerable()
            .GroupBy(row => row.Field<string>("username"))
            .Select(g => g.CopyToDataTable())
            .ToList();

            DataSet tables = new DataSet();

            foreach (var t in result)
            {
                t.TableName = t.Select().First().Field<string>(1);
                tables.Tables.Add(t);
            }                

            eu.ExportDataSetToExcel(tables, Application.StartupPath, Filename);

            //eu.WriteDataTableToExcel(dataTable,"Prueba","C:\\" + Filename,"Test");
            
            //io.WriteToFile(Filename, dr);
        }

        public void SaveOfflineAction(string sqlCommand)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;           
            command.CommandType = CommandType.Text;
            command.CommandText = sqlCommand;            
            try
            {
                conn.Open();               
                command.ExecuteNonQuery();
                conn.Close();                
            }
            catch (Exception ex)
            {
                //Creates the INSERT command to save in a file in case the connection drops
                var queryTest = CommandAsSql(command);
                IO io = new IO();
                io.WriteToFile(ConfigurationManager.AppSettings["BackupPath"], queryTest);
                throw new Exception("Error en conexion. Se ha registrado la actividad en forma local.\n\r" + ex.Message);
            }
        }

        public void SaveOfflineAction(StreamReader file)
        {
            string line;
            MySqlCommand command = new MySqlCommand();
            MySqlTransaction trans;
            conn.Open();
            trans = conn.BeginTransaction();
            command.Connection = conn;
            command.CommandType = CommandType.Text;            
            command.Transaction = trans;

            while ((line = file.ReadLine()) != null)
            {   
                command.CommandText = line;                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    trans.Rollback();
                    conn.Close();
                    throw new Exception(ex.Message);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    conn.Close();
                    throw new Exception("Error en conexion.Intente mas tarde.\n\r" + ex.Message);
                }
                
            }

            trans.Commit();
            conn.Close();
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
            command.Parameters.AddWithValue("@observaciones", observaciones == "Observaciones..." ? null : observaciones);
            Log.Info("Id de Usuario para insert = " + userId.ToString());

            try
            {
                conn.Open();
                //conn.Open();
                // ... other parameters
                command.ExecuteNonQuery();
            
                conn.Close();
            }
            catch(Exception ex)
            {
                //Creates the INSERT command to save in a file in case the connection drops
                var queryTest = CommandAsSql(command);                
                IO io = new IO();
                io.WriteToFile(ConfigurationManager.AppSettings["BackupPath"], queryTest);
                throw new Exception("Error en conexion. Se ha registrado la actividad en forma local.\n\r" + ex.Message);
            }            
        }        

        public string CommandAsSql(MySqlCommand sc)
        {
            string sql = sc.CommandText;

            sql = sql.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            sql = System.Text.RegularExpressions.Regex.Replace(sql, @"\s+", " ");

            foreach (MySqlParameter sp in sc.Parameters)
            {
                string spName = sp.ParameterName;
                string spValue = ParameterValueForSQL(sp);
                sql = sql.Replace(spName, spValue);
            }

            sql = sql.Replace("= NULL", "IS NULL");
            sql = sql.Replace("!= NULL", "IS NOT NULL");
            return sql;
        }

        public string ParameterValueForSQL(MySqlParameter sp)
        {
            string retval = "";

            switch (sp.MySqlDbType)
            {
                case MySqlDbType.Text:
                case MySqlDbType.Time:
                case MySqlDbType.VarChar:
                case MySqlDbType.Date:
                case MySqlDbType.DateTime:
                    if (sp.Value == null)
                    {
                        retval = "NULL";
                    }
                    else
                    {
                        retval = "'" + sp.Value.ToString().Replace("'", "''") + "'";
                    }
                    break;

                case MySqlDbType.Bit:
                    if (sp.Value == null)
                    {
                        retval = "NULL";
                    }
                    else
                    {
                        retval = ((bool)sp.Value == false) ? "0" : "1";
                    }
                    break;
                case MySqlDbType.Int32:
                    if (sp.Value == null)
                    {
                        retval = "NULL";
                    }
                    else 
                    {
                        retval = ((int)sp.Value).ToString();
                    }
                    break;
                default:
                    if (sp.Value == null)
                    {
                        retval = "NULL";
                    }
                    else
                    {
                        retval = sp.Value.ToString().Replace("'", "''");
                    }
                    break;
            }

            return retval;
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
