using Business;
using Business.Security;
using Presentation.Controls;
using Presentation.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Forms
{
    public partial class AddEditUserForm : Form
    {
        private int _userId = -1;
        private int _personId = -1;
        private FormMode _mode;

        public AddEditUserForm()
        {
            InitializeComponent();
            personDetailsWithFilterControl1.PersonSelected += PersonDetailsWithFilterControl1_PersonSelected;

            _mode = FormMode.Add;
        }

        public AddEditUserForm(int userId)
        {
            InitializeComponent();
            personDetailsWithFilterControl1.PersonSelected += PersonDetailsWithFilterControl1_PersonSelected;

            _mode = FormMode.Edit;
            _userId = userId;
            _personId = User.GetPersonId(_userId);
            personDetailsWithFilterControl1.PersonId = _personId;
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpPersonInfo;
            SwitchToMode();
            if (_mode == FormMode.Edit)
            {
                LoadUserDate();
            }
        }

        private void PersonDetailsWithFilterControl1_PersonSelected(object sender, PersonSavedEventArgs e)
        {
            _personId = (e != null && e.PersonId > 0) ? e.PersonId : -1;
        }

        private void LoadUserDate()
        {
            var user = User.Find(_userId);
            if (user != null)
            {
                lblUserId.Text = user.UserId.ToString();
                txtUserName.Text = user.UserName;
                chkIsActive.Checked = user.IsActive;
                txtPassword.Text = "*****";
                txtPassword.Enabled = false;
                txtConfirmPassword.Text = "*****";
                txtConfirmPassword.Enabled = false;
            }
        }

        private void SwitchToMode()
        {
            if (_mode == FormMode.Add)
            {
                this.Text = "Add New User";
                lblTitle.Text = "Add New User";
                personDetailsWithFilterControl1.EnableEditingOfPersonInfo = false;
            }
            else if (_mode == FormMode.Edit)
            {
                this.Text = "Update User";
                lblTitle.Text = "Update User";
                personDetailsWithFilterControl1.PersonFilter.Enabled = false;
                txtPassword.Enabled = false;
                txtConfirmPassword.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            User user;

            if (_mode == FormMode.Add)
            {
                // Username must be unique when adding
                if (User.IsUserExistByUserName(txtUserName.Text.Trim()))
                {
                    MessageBox.Show("Username already exists. Please choose another username.",
                        "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                user = new User();
                user.PersonId = _personId;
                user.UserName = txtUserName.Text.Trim();
                user.IsActive = chkIsActive.Checked;
                user.SetPassword(txtPassword.Text);
            }
            else // Edit
            {
                user = User.Find(_userId);
                if (user == null)
                {
                    MessageBox.Show("User not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                user.UserName = txtUserName.Text.Trim();
                user.IsActive = chkIsActive.Checked;
            }

            bool saved = user.Save();
            if (!saved)
            {
                MessageBox.Show("Failed to save user data.", "Save Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If this was Add, switch to Edit mode using newly created user id
            if (_mode == FormMode.Add)
            {
                _userId = user.UserId;
                lblUserId.Text = _userId.ToString();
                _mode = FormMode.Edit;
                SwitchToMode();
            }

            MessageBox.Show("Data Saved Successfully.", "Saved",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Checking that there is a person attached to the user
            if (_personId == -1)
            {
                MessageBox.Show("Please select a person to attach to the user", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            // Validating that Username is not empty
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Please select a username for new user", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            // Check if password is not empty
            string password = txtPassword.Text;
            if (_mode == FormMode.Add && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password cannot be empty", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            // Check if password confirmation matches the password.
            string inputConfirmPassword = txtConfirmPassword.Text;
            if (_mode == FormMode.Add && !string.Equals(password, inputConfirmPassword))
            {
                MessageBox.Show("Password Confirmation does not match Password!", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            return isValid;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // if the user already exists
            if (_personId != -1 && 
                User.IsUserExistByPersonId(_personId) &&
                _mode == FormMode.Add)
            {
                MessageBox.Show("Selected Person already has a user, choose another one.",
                    "Select Another Peron", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (_personId == -1 && _mode == FormMode.Add)
            {
                MessageBox.Show("Please select a person to attach to new user.",
                    "Select Peron", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabControl1.SelectedTab = tpLoginInfo;
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            string userName = txtUserName.Text;
            if (string.IsNullOrWhiteSpace(userName))
            {
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                errorProvider1.SetError(txtPassword, "Password cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            string password = txtPassword.Text;
            string inputConfirmPassword = txtConfirmPassword.Text;

            if (!string.Equals(password, inputConfirmPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        
    }
}
