using Business;
using Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Controls
{
    public partial class InternationalDrivingLicenseInformationControl : UserControl
    {
        public InternationalDrivingLicenseInformationControl()
        {
            InitializeComponent();
        }

        public int InternationalDrivingLicenseId
        {
            set
            {
                LoadData(value);
            }
        }

        private void LoadData(int internationalDrivingLicenseId)
        {

            if (internationalDrivingLicenseId <= 0)
                return;

            InternationalLicense internationalLicense = InternationalLicense.Find(internationalDrivingLicenseId);
            if (internationalLicense == null)
                return;

            lblInternationalDrivingLicenseId.Text = internationalLicense.InternationalLicenseId.ToString();
            lblLocalDrivingLicenseId.Text = internationalLicense.IssuedUsingLocalLicenseId.ToString();
            lblInternationalLicenseIssueDate.Text = internationalLicense.IssueDate.ToString("d");
            lblApplicationId.Text = internationalLicense.ApplicationId.ToString();
            lblIsActive.Text = internationalLicense.IsActive ? "Yes" : "No";
            lblDriverId.Text = internationalLicense.DriverId.ToString();
            lblLicenseExpirationDate.Text = internationalLicense.ExpirationDate.ToString("d");

            Driver driver = Driver.FindByDriverId(internationalLicense.DriverId);
            if (driver == null)
                return;

            Person person = Person.Find(driver.PersonId);
            if (person == null)
                return;

            string fullName = string.Format("{0} {1} {2}{3}",
                person.FirstName,
                person.SecondName,
                string.IsNullOrWhiteSpace(person.ThirdName) ? "" : person.ThirdName + " ",
                person.LastName);

            lblDriverName.Text = fullName;
            lblDriverNationalNumber.Text = person.NationalNo;
            lblDriverGender.Text = person.Gender == (byte)Person.PersonGender.Male ? "Male" : "Female";
            lblDriverDateOfBirth.Text = person.DateOfBirth.ToString("d");

            try
            {
                if (!string.IsNullOrWhiteSpace(person.ImagePath) && File.Exists(person.ImagePath))
                {
                    using (var img = Image.FromFile(person.ImagePath))
                    {
                        picDriverPicture.Image = new Bitmap(img);
                    }
                }
                else
                {
                    picDriverPicture.Image = person.Gender == (byte)Person.PersonGender.Male
                        ? Resources.person_default_image_male
                        : Resources.person_default_image_female;
                }
            }
            catch
            {
                picDriverPicture.Image = person.Gender == (byte)Person.PersonGender.Male
                    ? Resources.person_default_image_male
                    : Resources.person_default_image_female;
            }
        }
    }
}
