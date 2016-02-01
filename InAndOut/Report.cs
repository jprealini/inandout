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
            var date1 = date_from.Value.ToShortDateString();
            var date2 = date_to.Value.ToString("d/M/yyyy");
            var type = reportType_cmb.SelectedIndex;
            var timestamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            if(type > -1)
            {
                switch(type)
                {
                    case 0:
                        dAccess.GetRecords("Count_Hours", "ReporteTotales" + Global.appUser + "-" + timestamp, date1, date2, Global.appUserId);
                        break;
                    case 1:
                        dAccess.GetRecords("Raw_Report", "ReporteDetallado" + Global.appUser + "-" + timestamp, date1, date2, Global.appUserId);
                        break;
                    case 2:
                        dAccess.GetRecords("Count_Hours", "ReporteTotalesGeneral-" + timestamp, date1, date2);
                        break;
                    case 3:
                        dAccess.GetRecords("Raw_Report", "ReporteDetalladoGeneral-" + timestamp, date1, date2);
                        break;
                }           
            MessageBox.Show("El reporte ha sigo generado exitosamente");
            this.Close();
            }
            else
                MessageBox.Show("Seleccione un tipo de reporte");
            
        }
    }
}
