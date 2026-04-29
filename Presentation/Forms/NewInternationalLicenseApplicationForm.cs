using Business;
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
    public partial class NewInternationalLicenseApplicationForm : Form
    {
        private int _selectedLicenseId = -1;
        private int _internationalDrivingLicenseId = -1;
        private int _driverId = -1;

        public NewInternationalLicenseApplicationForm()
        {
            InitializeComponent();
            drivingLicenseInformationWithFilterControl1.LicenseSelected += DrivingLicenseInformationWithFilterControl1_LicenseSelected;
        }

        private void NewInternationalLicenseApplicationForm_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("d");
            lblInternationalLicenseExpirationDate.Text = DateTime.Now.ToString("d");
            lblApplicationFees.Text = Business.ApplicationType.GetApplicationTypeFees(
                Business.ApplicationType.ApplicationTypeTitle.NewInternationalDrivingLicense).ToString();
            Business.User user = Business.User.Find(Helpers.AppSession.CurrentUserId);
            lblCreatedByUserName.Text = user.UserName;
            lblInternationalLicenseExpirationDate.Text = DateTime.Now.AddYears(
                Business.InternationalLicense.InternationalLicenseValidityYears).ToString("d");
            lblInternationalLicenseIssueDate.Text = DateTime.Now.ToString("d");

            llbShowLicensesHistory.Enabled = false;
            llbShowLicensesInfo.Enabled = false;
            btnIssueInternationalDrivingLicense.Enabled = false;
        }

        private void DrivingLicenseInformationWithFilterControl1_LicenseSelected(object sender, LicenseSelectedEventArgs e)
        {
            _selectedLicenseId = e.LicenseId;

            Business.License license = Business.License.Find(_selectedLicenseId);

            // Rule 1: local license must be ordinary driving license (Class 3)
            if (license.LicenseClass != (int)LicenseClass.LicenseClassId.OrdinaryDrivingLicense)
            {
                MessageBox.Show("Selected license should be Class 3, select another one.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Rule 2: local license must be active
            else if (!license.IsActive)
            {
                MessageBox.Show("Selected license is not active, select another one.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Rule 3: local license must not be expired
            else if (license.ExpirationDate.Date < DateTime.Today)
            {
                MessageBox.Show("Selected license is expired, select another one.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Rule 4: local license must not be detained
            DriverLicenseInfo driverInfo = DriverLicenseInfo.GetByLicenseId(_selectedLicenseId);
            _driverId = driverInfo != null ? driverInfo.DriverId : -1;
            bool isDetained = driverInfo != null && driverInfo.IsDetained;
            if (isDetained)
            {
                MessageBox.Show("Selected license is currently detained, select another one.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Rule 5: local license must not have associated active international driving
            // license
            int activeInternationalLicenseId;
            if (InternationalLicense.TryGetActiveInternationalLicenseIdForLocalLicenseId(_selectedLicenseId, out activeInternationalLicenseId))
            {
                MessageBox.Show(
                    string.Format("This local license already has an active international driving license (ID = {0}).", activeInternationalLicenseId),
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Valid selection
            lblLocalDrivingLicenseId.Text = _selectedLicenseId.ToString();
            llbShowLicensesHistory.Enabled = true;
            btnIssueInternationalDrivingLicense.Enabled = true;
        }

        private void llbShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(_driverId);
            form.ShowDialog();
        }

        private void llbShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InternationalDrivingLicenseInformation form = new InternationalDrivingLicenseInformation(
                _internationalDrivingLicenseId);
            form.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssueInternationalDrivingLicense_Click(object sender, EventArgs e)
        {
            // Check if the driver have an international driving license or not
            Business.License localLicense = Business.License.Find(_selectedLicenseId);
            int driverId = localLicense.DriverID;

            int activeInternationalLicenseId;

            if (InternationalLicense.TryGetActiveInternationalLicenseIdForDriver(driverId, out activeInternationalLicenseId))
            {
                MessageBox.Show(
                    string.Format("This driver already has an active international driving license (ID = {0}).", activeInternationalLicenseId),
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (InternationalLicense.TryGetActiveInternationalLicenseIdForLocalLicenseId(_selectedLicenseId, out activeInternationalLicenseId))
            {
                MessageBox.Show(
                    string.Format("This local license already has an active international driving license (ID = {0}).", activeInternationalLicenseId),
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (InternationalLicense.DoesActiveInternationalLicenseExistForLocalLicenseId(_selectedLicenseId))
            {
                MessageBox.Show("This local license already has an active international driving license.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to issue the license?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Driver driver = Driver.FindByDriverId(driverId);
                int personId = driver.PersonId;

                // 1) Create application for international driving license
                Business.Application newApplication = new Business.Application()
                {
                    PersonId = personId,
                    ApplicationDate = DateTime.Now,
                    ApplicationTypeId = (int)ApplicationType.ApplicationTypeTitle.NewInternationalDrivingLicense,
                    ApplicationStatus = Business.Application.Status.New,
                    LastStatusDate = DateTime.Now,
                    PaidFees = ApplicationType.GetApplicationTypeFees(
                        ApplicationType.ApplicationTypeTitle.NewInternationalDrivingLicense),
                    UserId = Helpers.AppSession.CurrentUserId
                };

                if (!newApplication.Save())
                {
                    MessageBox.Show("Failed to create international license application.", "Fail",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2) Create international driving license
                DateTime issueDate = DateTime.Now;

                InternationalLicense newInternationalLicense = new InternationalLicense()
                {
                    ApplicationId = newApplication.ApplicationId,
                    DriverId = driverId,
                    IssuedUsingLocalLicenseId = _selectedLicenseId,
                    IssueDate = issueDate,
                    ExpirationDate = issueDate.AddYears(InternationalLicense.InternationalLicenseValidityYears),
                    IsActive = true,
                    CreatedByUserId = Helpers.AppSession.CurrentUserId
                };

                if (!newInternationalLicense.Save())
                {
                    // rollback parent application
                    Business.Application.DeleteApplication(newApplication.ApplicationId);

                    MessageBox.Show("Failed to issue international driving license.", "Fail",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _internationalDrivingLicenseId = newInternationalLicense.InternationalLicenseId;

                // 3) Mark application as completed
                newApplication.ApplicationStatus = Business.Application.Status.Completed;
                newApplication.LastStatusDate = DateTime.Now;
                newApplication.Save();

                // 4) Update UI
                lblInternationalLicenseApplicationId.Text = newApplication.ApplicationId.ToString();
                lblInternationalLicenseId.Text = _internationalDrivingLicenseId.ToString();
                lblInternationalLicenseIssueDate.Text = issueDate.ToString("d");
                lblInternationalLicenseExpirationDate.Text = newInternationalLicense.ExpirationDate.ToString("d");

                string text = string.Format("International license issued successfully with "
                    + "ID = {0}", _internationalDrivingLicenseId);
                MessageBox.Show(text, "License Issued", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                drivingLicenseInformationWithFilterControl1.LicenseFilterGroupBox.Enabled = false;
                btnIssueInternationalDrivingLicense.Enabled = false;
                llbShowLicensesInfo.Enabled = true;
            }
            else
            {
                return;
            }

        }
    }
}
