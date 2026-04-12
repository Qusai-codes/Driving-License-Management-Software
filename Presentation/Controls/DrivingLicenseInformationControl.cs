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
    public partial class DrivingLicenseInformationControl : UserControl
    {

        public DrivingLicenseInformationControl()
        {
            InitializeComponent();
        }

        public int DriverId
        {
            set
            {
                LoadDriverInformation(value);
            }
        }

        private void LoadDriverInformation(int driverId)
        {
            var driverInfo = DriverLicenseInfo.GetByDriverId(driverId);

            if (driverInfo == null)
            {
                lblLicenseClass.Text = "N/A";
                lblDriverName.Text = "N/A";
                lblLicenseId.Text = "N/A";
                lblDriverNationalNumber.Text = "N/A";
                lblDriverGender.Text = "N/A";
                lblLicenseIssueDate.Text = "N/A";
                lblLicenseIssueReason.Text = "N/A";
                lblLicenseNotes.Text = "N/A";
                lblIsActive.Text = "No";
                lblDriverDateOfBirth.Text = "N/A";
                lblDriverId.Text = "N/A";
                lblLicenseExpirationDate.Text = "N/A";
                lblIsLicenseDetained.Text = "No";

                picDriverLicenseHolder.Image = Resources.person_default_image_male;
                return;
            }

            lblLicenseClass.Text = LicenseClass.GetLicenseClassName(driverInfo.LicenseClassId);
            lblDriverName.Text = string.IsNullOrWhiteSpace(driverInfo.FullName) ? "N/A" : driverInfo.FullName;

            lblLicenseId.Text = driverInfo.LicenseId.ToString();

            lblDriverNationalNumber.Text = driverInfo.NationalNumber;

            lblDriverGender.Text = driverInfo.Gender == (byte)Person.PersonGender.Male ? "Male" : "Female";
            picDriverGender.Image = driverInfo.Gender == (byte)Person.PersonGender.Male ? Resources.Man_32 : Resources.Woman_32;

            lblLicenseIssueDate.Text = driverInfo.IssueDate.ToString("d");

            lblLicenseIssueReason.Text = GetIssueReasonText(driverInfo.IssueReason);

            lblLicenseNotes.Text = string.IsNullOrWhiteSpace(driverInfo.Notes)
                ? "No Notes"
                : driverInfo.Notes;

            lblIsActive.Text = driverInfo.IsActive ? "Yes" : "No";
            lblDriverDateOfBirth.Text = driverInfo.DateOfBirth.ToString("d");

            lblDriverId.Text = driverInfo.DriverId.ToString();

            lblLicenseExpirationDate.Text = driverInfo.ExpirationDate.ToString("d");

            lblIsLicenseDetained.Text = driverInfo.IsDetained ? "Yes" : "No";

            if (!string.IsNullOrWhiteSpace(driverInfo.ImagePath) && File.Exists(driverInfo.ImagePath))
            {
                picDriverLicenseHolder.Image = Image.FromFile(driverInfo.ImagePath);
            }
            else
            {
                picDriverLicenseHolder.Image = driverInfo.Gender == (byte)Person.PersonGender.Male
                    ? Resources.person_default_image_male
                    : Resources.person_default_image_female;
            }
        }

        private string GetIssueReasonText(byte issueReason)
        {
            if (!Enum.IsDefined(typeof(Business.License.IssueReasonType), issueReason))
            {
                return "Unknown";
            }

            switch ((Business.License.IssueReasonType)issueReason)
            {
                case Business.License.IssueReasonType.FirstTime:
                    return "First Time";
                case Business.License.IssueReasonType.Renew:
                    return "Renew";
                case Business.License.IssueReasonType.ReplacementForDamaged:
                    return "Replacement for Damaged";
                case Business.License.IssueReasonType.ReplacementForLost:
                    return "Replacement for Lost";
                default:
                    return "Unknown";
            }
        }
    }
}
