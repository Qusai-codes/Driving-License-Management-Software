using Business;
using Business.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Presentation.Helpers;

namespace Presentation
{
    public partial class LogInForm : Form
    {
        public User AuthenticatedUser { get; private set; }

        public LogInForm()
        {
            InitializeComponent();
        }

        private void LogInForm_Load(object sender, EventArgs e)
        {
            LoadSavedCredentials();
        }

        private void btnCloseLogInForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUserLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            string userName = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            User user = AuthenticateUser(userName, password);

            if (user != null)
            {
                AuthenticatedUser = user;
                AppSession.SetCurrentUser(user);
                SaveCredentialsIfNeeded();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                ShowValidationError("Please enter username.", txtUsername);
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowValidationError("Please enter password.", txtPassword);
                return false;
            }
            return true;
        }

        private User AuthenticateUser(string username, string password)
        {
            if (!User.IsUserExistByUserName(username))
            {
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            User user = User.Find(username);

            if (user == null)
            {
                return null;
            }

            if (!User.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                EventLogger.LogEventsToEventViewer("Invalid username and/or password" +
                    " please contact the administrator", EventLogEntryType.Warning);
                return null;
            }

            if (!user.IsActive)
            {
                MessageBox.Show("Your account is deactivated, please contact your admin.", 
                    "Inactive User",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return user;
        }

        private void ShowValidationError(string message, Control controlToFocus)
        {
            MessageBox.Show(message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            controlToFocus.Focus();
        }

        private void LoadSavedCredentials()
        {
            LoginCredentials credentials = CredentialsManager.Load();

            if (credentials.RememberMe)
            {
                txtUsername.Text = credentials.UserName;
                txtPassword.Text = credentials.Password;
                chkRememberUser.Checked = credentials.RememberMe;
                btnUserLogin.Focus();
            }
            else
            {
                txtUsername.Focus();
            }
        }

        private void SaveCredentialsIfNeeded()
        {
            string userName = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            bool rememberMe = chkRememberUser.Checked;

            if (!CredentialsManager.Save(userName, password, rememberMe))
            {
                MessageBox.Show("Warning: Could not save credentials.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
