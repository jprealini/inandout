﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Globalization;

namespace InAndOut
{
    public partial class Form1 : Form
    {
        Timer tmr = null;
        string dbType = ConfigurationManager.AppSettings["DatabaseType"];
        string station = ConfigurationManager.AppSettings["station"];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Login form = new Login();
            form.ShowDialog();
            //Inicia el timer que va a servir para fichar
            StartTimer();
            //Inicializar controles
            actual_user.Text = Global.appUser;                        
            currentTime.TextAlign = HorizontalAlignment.Center;
            observaciones_txt.Enabled = false;

            //Inicializar control de hora de ingreso
            hora_ingreso_dtp.Format = DateTimePickerFormat.Time;
            hora_ingreso_dtp.ShowUpDown = true;
            hora_ingreso_dtp.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);
            //Inicializar control de hora de egreso
            hora_egreso_dtp.Format = DateTimePickerFormat.Time;
            hora_egreso_dtp.ShowUpDown = true;
            hora_egreso_dtp.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 0, 0);
            
            GetStatus();
            
        }

        private void GetStatus()
        {
            var lastActivity = GetLastActivity();
            
            switch(lastActivity)
            {
                case 1:
                    in_button.Enabled = false;
                    out_button.Enabled = true;
                    currentTime.ForeColor = Color.Red;
                    break;
                case 2:
                    in_button.Enabled = true;
                    out_button.Enabled = false;
                    currentTime.ForeColor = Color.Green;
                    break;
                case 0:
                    in_button.Enabled = true;
                    out_button.Enabled = false;
                    currentTime.ForeColor = Color.Green;
                    break;                
            }            
        }

        private int GetLastActivity()
        {
            MySqlDataAccess dAccess = new MySqlDataAccess();
            var result = dAccess.ReadLastActivity("select actionId from Activity where userId = " + Global.appUserId + " order by id desc Limit 1");
                         
            return result;        
        }

        private void in_button_click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Confirma que el horario laboral de hoy es el seleccionado?", "Confirmacion de Horario", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                DateTime dt = DateTime.ParseExact(currentTime.Text, ConfigurationManager.AppSettings["DateTimeFormat"], CultureInfo.CurrentCulture);
                var diff = (dt - hora_ingreso_dtp.Value).TotalDays * 24 * 60;
                if (dt > hora_ingreso_dtp.Value && Math.Abs(diff) > 15)
                {
                    if (observaciones_txt.Text == "Observaciones...")
                        MessageBox.Show("Especifique en el campo Observaciones el motivo de su ingreso a esta hora");
                    else
                    {
                        DialogResult dialogResult2 = MessageBox.Show("Confirma que desea registrar el ingreso?", "Confirmacion de Ingreso", MessageBoxButtons.YesNo);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            RegisterEvent(Enums.Actions.IN, observaciones_txt.Text, station);
                            GetStatus();
                        }
                        else if (dialogResult2 == DialogResult.No)
                        {
                            GetStatus();
                        }
                    }
                }
                else
                {
                    DialogResult dialogResult3 = MessageBox.Show("Confirma que desea registrar el ingreso?", "Confirmacion de Ingreso", MessageBoxButtons.YesNo);
                    if (dialogResult3 == DialogResult.Yes)
                    {
                        RegisterEvent(Enums.Actions.IN, observaciones_txt.Text, station);
                        GetStatus();
                    }
                    else if (dialogResult3 == DialogResult.No)
                    {
                        GetStatus();
                    }                  
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                MessageBox.Show("Especifique correctamente el horario laboral del dia de hoy antes de marcar el Ingreso");
            }           
        }

        private void out_button_click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Confirma que el horario laboral de hoy es el seleccionado?", "Confirmacion de Horario", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                DateTime dt = DateTime.ParseExact(currentTime.Text, ConfigurationManager.AppSettings["DateTimeFormat"], CultureInfo.CurrentCulture);
                var diff = (dt - hora_egreso_dtp.Value).TotalDays * 24 * 60;
                if (dt < hora_egreso_dtp.Value && Math.Abs(diff) > 15)
                {
                    if (observaciones_txt.Text == "Observaciones...")
                        MessageBox.Show("Especifique en el campo Observaciones el motivo de su egreso a esta hora");
                    else
                    {
                        DialogResult dialogResult2 = MessageBox.Show("Confirma que desea registrar el egreso?", "Confirmacion de Egreso", MessageBoxButtons.YesNo);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            RegisterEvent(Enums.Actions.OUT, observaciones_txt.Text, station);
                            GetStatus();
                        }
                        else if (dialogResult2 == DialogResult.No)
                        {
                            GetStatus();
                        }
                    }
                }
                else
                {
                    DialogResult dialogResult3 = MessageBox.Show("Confirma que desea registrar el egreso?", "Confirmacion de Egreso", MessageBoxButtons.YesNo);
                    if (dialogResult3 == DialogResult.Yes)
                    {
                        if (observaciones_txt.Text == "Observaciones...")
                            MessageBox.Show("Especifique en el campo Observaciones el motivo de su egreso a esta hora");
                        else
                        {
                            RegisterEvent(Enums.Actions.OUT, observaciones_txt.Text, station);
                            GetStatus();
                        }                        
                    }
                    else if (dialogResult3 == DialogResult.No)
                    {
                        GetStatus();
                    }
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                MessageBox.Show("Especifique correctamente el horario laboral del dia de hoy antes de marcar el Ingreso");
            }                  
        }

        private void RegisterEvent(Enums.Actions action, string observaciones, string station)
        {            
            MySqlDataAccess mySqlDAccess = new MySqlDataAccess();
            string mySqlcommand = "INSERT INTO Activity (userId, fecha, hora, actionId, estacion, observaciones)";
            mySqlcommand += " VALUES (@userId, @fecha, @hora, @actionId, @station, @observaciones)";
            mySqlDAccess.SaveAction(mySqlcommand, Global.appUserId, action, currentTime.Text, observaciones, station);
        }

        private void StartTimer()
        {
            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 1000;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Enabled = true;
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            DateTimeFormatInfo fmt = (new CultureInfo("es-ES")).DateTimeFormat;
            currentTime.Text = DateTime.Now.ToString("g", fmt);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.appUser = "";
            actual_user.Text = "";
            Login form = new Login();
            form.ShowDialog();
            actual_user.Text = Global.appUser;
            loginToolStripMenuItem.Text = "Logout";
            GetStatus();
        }

        private void registrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.appUser = "";
            actual_user.Text = ""; 
            Register form = new Register();
            form.ShowDialog();            
            loginToolStripMenuItem.Text = "Login";
            in_button.Enabled = false;
            out_button.Enabled = false;
        }

        private void generarReporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report form = new Report();
            form.ShowDialog();
        }

        private void ingresoManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateAndTimeOverride form = new DateAndTimeOverride();
            form.ShowDialog();
        }

        private void hora_ingreso_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (currentTime.Text != "")
            {
                DateTime dt = DateTime.ParseExact(currentTime.Text, ConfigurationManager.AppSettings["DateTimeFormat"], CultureInfo.CurrentCulture);

                if (dt > hora_ingreso_dtp.Value)
                    observaciones_txt.Enabled = true;
                else
                    observaciones_txt.Enabled = false;
            }
        }

        private void hora_egreso_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (currentTime.Text != "")
            {
                DateTime dt = DateTime.ParseExact(currentTime.Text, ConfigurationManager.AppSettings["DateTimeFormat"], CultureInfo.CurrentCulture);

                if (dt < hora_egreso_dtp.Value)
                    observaciones_txt.Enabled = true;
                else
                    observaciones_txt.Enabled = false;
            }
        }       
    }
}
