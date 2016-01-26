using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;

namespace InAndOut
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            this.ActiveControl = user_txtBox;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            string user = user_txtBox.Text;
            MySqlDataAccess dAccess = new MySqlDataAccess();

            byte[] passwordBytes = Encoding.Unicode.GetBytes(pass_txtBox.Text);
            //
            // This is where you'd normally append or prepend the salt bytes
            //
            var hasher = System.Security.Cryptography.SHA256.Create();
            byte[] hashedBytes = hasher.ComputeHash(passwordBytes);

            //string command = "Update Users set password = HASHBYTES('sha2_256', @password)";
            string command = "Update Users set password = @password";
            command += " where userName = @userName";

            dAccess.RegisterUser(command, user, hashedBytes);

            this.Close();            
        }
    }
}
