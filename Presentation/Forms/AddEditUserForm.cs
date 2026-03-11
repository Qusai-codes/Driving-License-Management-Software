using Business;
using Business.Security;
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

        public AddEditUserForm(FormMode mode, int userId = -1)
        {
            InitializeComponent();
            personDetailsViewControl1.EditPersonInfoClicked += PersonDetailsViewControl_EditPersonInfoClicked;
            _mode = mode;
            if (_mode == FormMode.Edit)
            {
                _userId = userId;
                _personId = User.GetPersonId(_userId);
                LoadPersonData(_personId);
            }
        }

        private void PersonDetailsViewControl_EditPersonInfoClicked(object sender, EventArgs e)
        { 
            if (_personId != -1)
            {
                PersonProfileForm personProfile = new PersonProfileForm(FormMode.Edit, _personId);
                personProfile.ShowDialog();

                // Reload person data after edit
                LoadPersonData(_personId);
            }
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpPersonInfo;
            SwitchToMode(_mode);
            if (_mode == FormMode.Edit)
            {
                LoadPersonData(_personId);
                LoadUserDate();
            }
            SetUpPersonFilterCombo();
        }

        private void SetUpPersonFilterCombo()
        {
            cmbFilter.Items.Clear();
            cmbFilter.Items.AddRange(new object[] { "National No.", "Person ID" });
            cmbFilter.SelectedIndex = 0;
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

        private void LoadPersonData(int personId)
        {
            Person person = Person.Find(personId);
            if (person == null)
            {
                MessageBox.Show("Person not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            personDetailsViewControl1.EnableEditingOfPersonInfo = true;
            personDetailsViewControl1.PersonId = person.PersonId;
            string fullName = string.Format("{0} {1} {2}{3}",
                person.FirstName, person.SecondName,
                string.IsNullOrEmpty(person.ThirdName) ? "" : person.ThirdName + " ",
                person.LastName);
            personDetailsViewControl1.FullName = fullName;
            personDetailsViewControl1.NationalNo = person.NationalNo;
            personDetailsViewControl1.Gender = person.Gender == 0 ? "Male" : "Female";
            personDetailsViewControl1.Email = string.IsNullOrEmpty(person.Email) ?
                                                    "" : person.Email;
            personDetailsViewControl1.Address = person.Address;
            personDetailsViewControl1.DateOfBirth = person.DateOfBirth;
            personDetailsViewControl1.Phone = person.Phone;
            personDetailsViewControl1.Country = Country.GetCountryNameById(person.NationalityCountryID);

            // Get person image
            try
            {
                using (var img = Image.FromFile(person.ImagePath))
                {
                    personDetailsViewControl1.PersonImage.Image = new Bitmap(img);
                }

            }
            catch (Exception ex) { }
        }

        private void SwitchToMode(FormMode mode)
        {
            _mode = mode;
            

            if (_mode == FormMode.Add)
            {
                this.Text = "Add New User";
                lblTitle.Text = "Add New User";
                personDetailsViewControl1.EnableEditingOfPersonInfo = false;
            }
            else if (_mode == FormMode.Edit)
            {
                this.Text = "Update User";
                lblTitle.Text = "Update User";
                grpFilterPerson.Enabled = false;
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
                SwitchToMode(FormMode.Edit);
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

        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            string selectedFilter = cmbFilter.SelectedItem?.ToString();
            string filterValue = txtFilterValue.Text.Trim();

            if (string.IsNullOrWhiteSpace(filterValue))
            {
                MessageBox.Show("Please enter a value to search.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Person person = null;

            if (selectedFilter == "National No.")
            {
                person = Person.Find(filterValue);
                
                if (person == null)
                {
                    MessageBox.Show($"No Person with National No. = {filterValue}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (selectedFilter == "Person ID")
            {
                int personId;
                if (!int.TryParse(filterValue, out personId))
                {
                    MessageBox.Show("Person ID must be numeric.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                person = Person.Find(personId);
                if (person == null)
                {
                    MessageBox.Show($"No Person with Person ID = {filterValue}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (person == null)
            {
                personDetailsViewControl1.ResetView();
                return;
            }
            _personId = person.PersonId;
            LoadPersonData(_personId);
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            using (PersonProfileForm personProfileForm = new PersonProfileForm(FormMode.Add))
            {
                EventHandler<PersonSavedEventArgs> onPersonSaved = (s, args) =>
                {
                    _personId = args.PersonId;
                };

                personProfileForm.PersonSaved += onPersonSaved;
                personProfileForm.ShowDialog();
                personProfileForm.PersonSaved -= onPersonSaved;
            }

            if (_personId != -1)
            {
                LoadPersonData(_personId);
            }
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
