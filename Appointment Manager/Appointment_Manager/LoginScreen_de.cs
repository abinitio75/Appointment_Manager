using System;
using System.Windows.Forms;
using System.Globalization;

namespace Appointment_Manager
{
    public partial class LoginScreen_de : Form
    {
        private string loginFail = "There was an error logging in. Check login credentials.";

        public LoginScreen_de()
        {
            if (CultureInfo.CurrentCulture.Name.Contains("de"))
            {
                loginFail = "Beim Einloggen ist ein Fehler aufgetreten. Anmeldedaten prüfen.";
            }
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            btnLogin.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUserName.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                bool valid = User.Login(txtUserName.Text, txtPassword.Text);

                if (valid)
                {
                    User.SetCurrentUser(txtUserName.Text);
                    Common.WriteToLog(true);
                    txtUserName.Text = null;
                    txtPassword.Text = null;
                    Hide();
                    new MainScreen().ShowDialog();
                    Common.WriteToLog(false);
                    User.SetUserID(0);
                    User.SetUserName(null);
                    
                    if (!IsDisposed)
                    {
                        Show();
                    }
                }
                else
                {
                     MessageBox.Show(loginFail);
                }
            }
        }
        
        private void txtUserName_Enter(object sender, EventArgs e)
        {
            if (txtUserName.Text == "Username")
                txtUserName.Clear();
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
                txtUserName.Text = "Username";
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.PasswordChar = '*';
                txtPassword.Clear();
            }
        }
        
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.PasswordChar = '\0';
                txtPassword.Text = "Password";
            }
        }

        private void btnExit_Click(object sender, EventArgs e) => Application.Exit();

        private void LoginScreen_Load(object sender, EventArgs e)
        {
            txtUserName.MaxLength = 50;
            txtPassword.MaxLength = 50;
            btnLogin.Focus();
        }

        private void cbxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = cbxShowPassword.Checked ? '\0' : '*';
        }
    }
}