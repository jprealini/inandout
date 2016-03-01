using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InAndOut
{
    public partial class Report : Form
    {
        public Report()
        {            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlDataAccess dAccess = new MySqlDataAccess();
            var date1 = date_from.Value.ToString("d/M/yyyy");
            var date2 = date_to.Value.ToString("d/M/yyyy");
            var type = reportType_cmb.SelectedIndex;
            var timestamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            if(type > -1)
            {
                switch(type)
                {
                    //case 0:
                    //    dAccess.GetRecords("Count_Hours_2", @"\ReporteTotales" + Global.appUser + "-" + timestamp + ".xlsx", date1, date2, Global.appUserId);
                    //    break;
                    //case 1:
                    //    dAccess.GetRecords("Raw_Report_2", @"\ReporteDetallado" + Global.appUser + "-" + timestamp + ".xlsx", date1, date2, Global.appUserId);
                    //    break;
                    case 0:
                        dAccess.GetRecords("Count_Hours_2", @"\ReporteTotales-" + timestamp + ".xlsx", date1, date2);
                        break;
                    case 1:
                        dAccess.GetRecords("Raw_Report_2", @"\ReporteDetallado-" + timestamp + ".xlsx", date1, date2);
                        break;
                }           
            MessageBox.Show("El reporte ha sido generado exitosamente");
            
            }
            else
                MessageBox.Show("Seleccione un tipo de reporte");
            
        }
    }
}
