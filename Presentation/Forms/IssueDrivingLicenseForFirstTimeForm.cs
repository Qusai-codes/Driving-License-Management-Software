using Business;
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

namespace Presentation.Forms
{
    public partial class IssueDrivingLicenseForFirstTimeForm : Form
    {
        private int _localDrivingLicenseApplicationId;
        public IssueDrivingLicenseForFirstTimeForm(int localDrivingLicenseApplicationId)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IssueDrivingLicenseForFirstTimeForm_Load(object sender, EventArgs e)
        {
            drivingLicenseApplicationInformationControl1.LocalDrivingLicenseApplicationId =
                _localDrivingLicenseApplicationId;
        }

        private void btnIssueDrivingLicense_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplication localApp =
                LocalDrivingLicenseApplication.Find(_localDrivingLicenseApplicationId);

            if (localApp == null)
            {
                MessageBox.Show("Local application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Business.Application app = Business.Application.FindByApplicationId(localApp.ApplicationId);
            if (app == null)
            {
                MessageBox.Show("Application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Business.License.DoesLicenseExist(app.ApplicationId))
            {
                MessageBox.Show("A license is already issued for this application.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int driverId = EnsureDriverId(app.PersonId, localApp.LicenseClassId, out bool alreadyHasClassLicense);

            if (driverId == -1)
            {
                MessageBox.Show("Unable to create/find driver record.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (alreadyHasClassLicense)
            {
                MessageBox.Show("This driver already has an active license for the selected class.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime issueDate = DateTime.Now;

            Business.License newLicense = new Business.License
            {
                ApplicationID = app.ApplicationId,
                DriverID = driverId,
                LicenseClass = localApp.LicenseClassId,
                IssueDate = issueDate,
                ExpirationDate = issueDate.AddYears(LicenseClass.GetMinimumAllowedAge(localApp.LicenseClassId)),
                Notes = textBox1.Text.Trim(),
                PaidFees = app.PaidFees,
                IsActive = true,
                IssueReason = (byte)Business.License.IssueReasonType.FirstTime,
                CreatedByUserID = AppSession.CurrentUserId
            };

            if (!newLicense.Save())
            {
                MessageBox.Show("Failed to issue license.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("License issued Successfully with License ID = " + newLicense.LicenseID,
                "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private int EnsureDriverId(int personId, int licenseClassId, out bool alreadyHasClassLicense)
        {
            alreadyHasClassLicense = false;

            DataTable drivers = Driver.GetAllDrivers();
            int driverId = -1;

            if (drivers != null && drivers.Rows.Count > 0)
            {
                DataRow existing = drivers.AsEnumerable()
                    .FirstOrDefault(r => r.Field<int>("PersonID") == personId);

                if (existing != null)
                    driverId = existing.Field<int>("DriverID");
            }

            if (driverId == -1)
            {
                Driver newDriver = new Driver
                {
                    PersonId = personId,
                    CreatedByUserId = AppSession.CurrentUserId,
                    CreatedDate = DateTime.Now
                };

                if (!newDriver.Save())
                    return -1;

                drivers = Driver.GetAllDrivers();
                DataRow created = drivers.AsEnumerable()
                    .Where(r => r.Field<int>("PersonID") == personId)
                    .OrderByDescending(r => r.Field<int>("DriverID"))
                    .FirstOrDefault();

                driverId = created != null ? created.Field<int>("DriverID") : -1;
            }

            if (driverId != -1)
            {
                alreadyHasClassLicense = Business.License.DoesActiveLicenseExistForDriverAndClass(driverId, licenseClassId);
            }

            return driverId;
        }

    }
}
