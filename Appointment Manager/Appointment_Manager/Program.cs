using System;
using System.Windows.Forms;
using System.Globalization;

namespace Appointment_Manager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (CultureInfo.CurrentCulture.LCID != 1033)
                CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            
            DBConnection db = new DBConnection(true);

            //db.DeleteDB();
            
            if (db.IsDBCreated() && !db.fail)
            {
                Application.Run(new LoginScreen());
            }
            else
            {
                MessageBox.Show("Fatal database problem. Exiting.");
            }
        }
    }
}
