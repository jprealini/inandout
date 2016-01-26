using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace InAndOut
{
    public partial class PrepareReport : Form
    {
        public PrepareReport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseSourceFolder();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChooseDestinationFolder();
        }

        public void ChooseSourceFolder()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                source_txt.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        public void ChooseDestinationFolder()
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                destination_txt.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                var srcDirectoryName = @"C:\Reportes";
                var destDirectoryName = @"C:\Reportes\Merge\ReporteConsolidado" + mes_cmb.SelectedItem.ToString() + ".csv";


                var allCsv = Directory.EnumerateFiles(srcDirectoryName, "*", System.IO.SearchOption.TopDirectoryOnly);
                string[] header = { File.ReadLines(allCsv.First()).First(l => !string.IsNullOrWhiteSpace(l)) };
                var mergedData = allCsv
                    .SelectMany(csv => File.ReadLines(csv)
                        .SkipWhile(l => string.IsNullOrWhiteSpace(l)).Skip(1)); // skip header of each file
                File.WriteAllLines(destDirectoryName, header.Concat(mergedData));
                MessageBox.Show("El reporte consolidado se ha generado exitosamente...");
                IO io = new IO();
                var dataTable = io.GetDataTabletFromCSVFile(destDirectoryName);
                //var dataTable = GetDataTabletFromCSVFile(destDirectoryName);
                MySqlDataAccess da = new MySqlDataAccess();
                da.InsertDataIntoMySQLServerUsingBulkCopy(dataTable);
                this.Close();
            }
        }

        private bool Validar()
        {
            if (source_txt.TextLength == 0)
            {
                MessageBox.Show("Debe seleccionar una carpeta de origen de reportes...");
                return false;
            }

            if (destination_txt.TextLength == 0)
            {
                MessageBox.Show("Debe seleccionar una carpeta de destino para el consolidado...");
                return false;
            }

            if (mes_cmb.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar el mes correspondiente...");
                return false;
            }

            return true;
        }
    }
}

