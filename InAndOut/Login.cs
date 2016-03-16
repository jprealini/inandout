using System;
using System.Text;
using System.Windows.Forms;

namespace InAndOut
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            ActiveControl = user_txtBox;
            pass_txtBox.PasswordChar = '*';
            user_txtBox.CharacterCasing = CharacterCasing.Lower;
            // Align the text in the center of the TextBox control.
            pass_txtBox.TextAlign = HorizontalAlignment.Center;
            user_txtBox.TextAlign = HorizontalAlignment.Center;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();            
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            MySqlDataAccess dAccess = new MySqlDataAccess();
            //var hashedString = Convert.ToBase64String(hashedBytes);

            byte[] passwordBytes = Encoding.Unicode.GetBytes(pass_txtBox.Text);
            //
            // This is where you'd normally append or prepend the salt bytes
            //
            var hasher = System.Security.Cryptography.SHA256.Create();
            byte[] hashedBytes = hasher.ComputeHash(passwordBytes);

            var result = dAccess.GetUser("select * from Users where UserName = @userName and Password = @password", user_txtBox.Text, hashedBytes);
            if (!result)
            {
                ClearTextBoxes();
                MessageBox.Show("Usuario o contraseña incorrectos... vuelva a intentar");
            }
            else            
                this.Close();
        }

        private void ClearTextBoxes()
        {
            user_txtBox.Clear();
            pass_txtBox.Clear();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
