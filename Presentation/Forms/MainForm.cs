using Business;
using Presentation.Forms;
using Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class MainForm : Form
    {
        private User _currentUser;

        public MainForm(User authenticatedUser = null)
        {
            InitializeComponent();
            _currentUser = authenticatedUser;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManagePeopleForm managePeopleForm = new ManagePeopleForm();
            managePeopleForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int loggedInUserId = AppSession.CurrentUserId;
            UserDetailsForm form = new UserDetailsForm(loggedInUserId);
            form.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePasswordForm form = new ChangePasswordForm(_currentUser.UserId);
            form.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to sign out?",
                "Confirm Sign Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                AppSession.Clear();
                // Hide the main form
                this.Hide();

                // Show login form
                LogInForm loginForm = new LogInForm();

                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    _currentUser = loginForm.AuthenticatedUser;
                    this.Show();
                }
                else
                {
                    // Login cancelled or failed - close the application
                    this.Close();
                }
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageUsersForm form = new ManageUsersForm();
            form.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageApplicationTypesForm form = new ManageApplicationTypesForm();
            form.ShowDialog();
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestTypesForm form = new TestTypesForm();
            form.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseForm form = new LocalDrivingLicenseForm(
                FormMode.Add, _currentUser.UserId);
            form.ShowDialog();
        }

        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplicationsForm form =
                new LocalDrivingLicenseApplicationsForm(_currentUser.UserId);
            form.ShowDialog();
        }
    }
}
