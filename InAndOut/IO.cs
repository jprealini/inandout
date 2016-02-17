using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Web;

namespace InAndOut
{
    public class IO
    {
        public void WriteToFile(string Filename, SqlDataReader dr)
        {
            Filename = @"C:\Reportes\" + Filename;
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(Filename))
            {
                // Loop through the fields and add headers
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    if (name.Contains(","))
                        name = "\"" + name + "\"";

                    if (i != dr.FieldCount - 1)
                        fs.Write(name + ",");
                    else
                        fs.Write(name);
                }
                fs.WriteLine();

                // Loop through the rows and output the data
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        if (i != dr.FieldCount - 1)
                            fs.Write(value + ",");
                        else
                            fs.Write(value);

                    }
                    fs.WriteLine();
                }

                fs.Close();
            }
        }

        public void WriteToFile(string Filename, MySqlDataReader dr)
        {
            Filename = @"C:\Reportes\" + Filename;
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(Filename))
            {
                // Loop through the fields and add headers
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    if (name.Contains(","))
                        name = "\"" + name + "\"";

                    if (i != dr.FieldCount - 1)
                        fs.Write(name + ",");
                    else
                        fs.Write(name);
                }
                fs.WriteLine();

                // Loop through the rows and output the data
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        if (i != dr.FieldCount - 1)
                            fs.Write(value + ",");
                        else
                            fs.Write(value);

                    }
                    fs.WriteLine();
                }

                fs.Close();
            }
        }

        /// <summary>
        /// This method writes to a file the insert commands saved in case the connection drops when saving an activity, 
        /// in order to use them later to insert them in the db
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="command"></param>
        public void WriteToFile(string Filename, string command)
        {
            // Compose a string that consists of three lines.
            string lines = command;

            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter(Filename, true);
            file.WriteLine(lines);

            file.Close();
        }

        public DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return csvData;
        }

    }
}
