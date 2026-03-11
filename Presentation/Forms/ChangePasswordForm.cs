using Business;
using Presentation.Controls;
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
    public partial class ChangePasswordForm : Form
    {
        private int _userId;
        private string _userName;
        public ChangePasswordForm(int userId)
        {
            InitializeComponent();
            userDetailsControl1.PersonDetailsControl.EditPersonInfoClicked += PersonDetailsViewControl_EditPersonInfoClicked;
            _userId = userId;
        }

        private void PersonDetailsViewControl_EditPersonInfoClicked(object sender, EventArgs e)
        {
            int personId = User.GetPersonId(_userId);
            PersonProfileForm personProfile = new PersonProfileForm(FormMode.Edit, personId);
            personProfile.ShowDialog();

            // Reload person data after edit
            LoadPersonData();
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {
            LoadPersonData();
            LoadUserDate();
        }

        private void LoadUserDate()
        {
            var user = User.Find(_userId);
            if (user != null)
            {
                _userName = user.UserName;
                userDetailsControl1.UserId = user.UserId.ToString();
                userDetailsControl1.UserName = user.UserName;
                userDetailsControl1.IsActive = user.IsActive ? "Yes" : "No";
            }
        }

        private void LoadPersonData()
        {
            var personDetailsViewControl = userDetailsControl1.PersonDetailsControl;
            int personId = User.GetPersonId(_userId);
            Person person = Person.Find(personId);
            if (person == null)
            {
                MessageBox.Show("Person not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            personDetailsViewControl.PersonId = person.PersonId;
            string fullName = string.Format("{0} {1} {2}{3}",
                person.FirstName, person.SecondName,
                string.IsNullOrEmpty(person.ThirdName) ? "" : person.ThirdName + " ",
                person.LastName);
            personDetailsViewControl.FullName = fullName;
            personDetailsViewControl.NationalNo = person.NationalNo;
            personDetailsViewControl.Gender = person.Gender == 0 ? "Male" : "Female";
            personDetailsViewControl.Email = string.IsNullOrEmpty(person.Email) ?
                                                    "" : person.Email;
            personDetailsViewControl.Address = person.Address;
            personDetailsViewControl.DateOfBirth = person.DateOfBirth;
            personDetailsViewControl.Phone = person.Phone;
            personDetailsViewControl.Country = Country.GetCountryNameById(person.NationalityCountryID);

            // Get person image
            try
            {
                using (var img = Image.FromFile(person.ImagePath))
                {
                    personDetailsViewControl.PersonImage.Image = new Bitmap(img);
                }

            }
            catch (Exception ex) { }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            User user = User.Find(_userId);
            if (user != null)
            {
                string newPassword = txtNewPassword.Text;
                string oldPassword = txtCurrentPassword.Text;
                if (user.ChangePassword(newPassword, oldPassword))
                {
                    MessageBox.Show("Password Changed Successfully.",
                        "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCurrentPassword.Clear();
                    txtNewPassword.Clear();
                    txtConfirmNewPassword.Clear();

                    CredentialsManager.SavePasswordOnly(newPassword);
                }
                else
                {
                    MessageBox.Show("Unable to Save New Password.",
                        "Fail.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            // check if the entered password is wrong
            string inputPassword = txtCurrentPassword.Text;

            if (string.IsNullOrEmpty(_userName) || !User.CheckPassword(_userName, inputPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password is wrong!");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            string inputPassword = txtNewPassword.Text;

            if (string.IsNullOrWhiteSpace(inputPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "New password cannot be empty.");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void txtConfirmNewPassword_Validating(object sender, CancelEventArgs e)
        {
            string inputConfirmPassword = txtConfirmNewPassword.Text;
            string inputNewPassword = txtNewPassword.Text;

            if (!string.Equals(inputNewPassword, inputConfirmPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmNewPassword, "The confirmed password does not match the new one.");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }
    }
}
