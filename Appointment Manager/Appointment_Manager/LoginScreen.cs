using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Appointment_Manager
{
    public partial class LoginScreen : Form
    {
        private string loginFail = "There was an error logging in. Check login credentials.";

        public LoginScreen()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
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
