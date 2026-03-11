using Business;
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
    public partial class UserDetailsForm : Form
    {
        private int _userId;
        public UserDetailsForm(int userId)
        {
            InitializeComponent();
            userDetailsControl1.PersonDetailsControl.EditPersonInfoClicked += PersonDetailsViewControl_EditPersonInfoClicked;
            _userId = userId;
        }

        private void UserDetailsForm_Load(object sender, EventArgs e)
        {
            LoadPersonData();
            LoadUserDate();
        }

        private void LoadUserDate()
        {
            var user = User.Find(_userId);
            if (user != null)
            {
                userDetailsControl1.UserId = user.UserId.ToString();
                userDetailsControl1.UserName = user.UserName;
                userDetailsControl1.IsActive = user.IsActive ? "Yes" : "No";
            }
        }

        private void PersonDetailsViewControl_EditPersonInfoClicked(object sender, EventArgs e)
        {
            int personId = User.GetPersonId(_userId);
            PersonProfileForm personProfile = new PersonProfileForm(FormMode.Edit, personId);
            personProfile.ShowDialog();

            // Reload person data after edit
            LoadPersonData();
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

        
    }
}
