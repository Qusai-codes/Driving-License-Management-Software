using Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Business;

namespace Presentation
{
    public partial class PersonDetailsForm : Form
    {
        private int _personId;

        public PersonDetailsForm(int personId)
        {
            InitializeComponent();
            personDetailsViewControl1.EditPersonInfoClicked += PersonDetailsViewControl_EditPersonInfoClicked;
            _personId = personId;
        }

        private void PersonDetailsViewControl_EditPersonInfoClicked(object sender, EventArgs e)
        {
            PersonProfileForm personProfile = new PersonProfileForm(FormMode.Edit, _personId);
            personProfile.ShowDialog();

            // Reload person data after edit
            LoadPersonData();
        }

        private void PersonDetailsForm_Load(object sender, EventArgs e)
        {
            LoadPersonData();
        }

        private void LoadPersonData()
        {
            Person person = Person.Find(_personId);
            if (person == null)
            {
                MessageBox.Show("Person not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // TODO: complete the implementation
            // Populate your view controls with person data
            // Example:
            // personDetailsViewControl.FirstName = person.FirstName;
            // personDetailsViewControl.LastName = person.LastName;
            // etc.
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
