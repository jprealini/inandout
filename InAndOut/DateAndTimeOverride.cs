using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InAndOut
{
    public partial class DateAndTimeOverride : Form
    {
        public DateAndTimeOverride()
        {
            InitializeComponent();
            hora.Format = DateTimePickerFormat.Time;
            hora.ShowUpDown = true;
            
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fecha = date.Value.ToShortDateString();
            var horario = hora.Value.ToShortTimeString();

        }
    }
}
