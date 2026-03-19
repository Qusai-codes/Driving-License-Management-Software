using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Business;

namespace Presentation
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            if (User.HasUsers())
            {
                LogInForm logInForm = new LogInForm();

                if (logInForm.ShowDialog() == DialogResult.OK)
                {
                    // Login successful, show main form with authenticated user
                    System.Windows.Forms.Application.Run(new MainForm(logInForm.AuthenticatedUser));
                }

            }
            else
            {
                System.Windows.Forms.Application.Run(new MainForm());
            }
            
        }
    }
}
