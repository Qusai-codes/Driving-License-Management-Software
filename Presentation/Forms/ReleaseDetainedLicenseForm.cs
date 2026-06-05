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
    public partial class ReleaseDetainedLicenseForm : Form
    {
        private int _licenseId;
        private DetainedLicense _detainedLicense;

        public ReleaseDetainedLicenseForm()
        {
            InitializeComponent();
            drivingLicenseInformationWithFilterControl1.LicenseSelected += DrivingLicenseInformationWithFilterControl1_LicenseSelected;
        }

        public ReleaseDetainedLicenseForm(int licenseId)
        {
            InitializeComponent();
            drivingLicenseInformationWithFilterControl1.LicenseSelected += DrivingLicenseInformationWithFilterControl1_LicenseSelected;

            if (licenseId > 0)
            {
                drivingLicenseInformationWithFilterControl1.TrySelectLicense(licenseId);
                drivingLicenseInformationWithFilterControl1.LicenseFilterGroupBox.Enabled = false;
            }
        }

        private void ReleaseDetainedLicenseForm_Load(object sender, EventArgs e)
        {
            lblApplicationFees.Text = ApplicationType.GetApplicationTypeFees(
                ApplicationType.ApplicationTypeTitle.ReleaseDetainedLicense).ToString();

            lblCreatedByUserName.Text = AppSession.CurrentUser.UserName;

            llbShowLicensesHistory.Enabled = false;
            llbShowReleasedLicenseInfo.Enabled = false;
            btnRelease.Enabled = false;
        }

        private void DrivingLicenseInformationWithFilterControl1_LicenseSelected(object sender, Events.LicenseSelectedEventArgs e)
        {
            _licenseId = e.LicenseId;

            Business.License currentLicense = Business.License.Find(_licenseId);
            if (currentLicense == null)
            {
                MessageBox.Show("Selected license was not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!DetainedLicense.IsLicenseDetained(_licenseId))
            {
                MessageBox.Show("Selected license is not detained, choose another one.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _detainedLicense = DetainedLicense.FindByLicenseId(_licenseId);
            if (_detainedLicense == null || _detainedLicense.IsReleased)
            {
                MessageBox.Show("Detained license information not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update UI
            lblDetainId.Text = _detainedLicense.DetainId.ToString();
            lblDetainDate.Text = _detainedLicense.DetainDate.ToString("dd/MMM/yyyy");
            lblLicenseId.Text = _licenseId.ToString();
            lblFineFees.Text = _detainedLicense.FineFees.ToString();

            decimal applicationFees = ApplicationType.GetApplicationTypeFees(
                ApplicationType.ApplicationTypeTitle.ReleaseDetainedLicense);
            lblApplicationFees.Text = applicationFees.ToString();
            lblTotalFees.Text = (applicationFees + _detainedLicense.FineFees).ToString();

            llbShowLicensesHistory.Enabled = true;
            btnRelease.Enabled = true;
        }

        private void llbShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Business.License currentLicense = Business.License.Find(_licenseId);
            if (currentLicense == null)
            {
                return;
            }

            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(currentLicense.DriverID);
            form.ShowDialog();
        }

        private void llbShowReleasedLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_licenseId <= 0)
            {
                return;
            }

            DriverLicenseInformationForm form = new DriverLicenseInformationForm(_licenseId);
            form.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (_licenseId <= 0 || _detainedLicense == null)
            {
                MessageBox.Show("Please select a detained license first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_detainedLicense.IsReleased)
            {
                MessageBox.Show("Selected license is already released.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Business.License currentLicense = Business.License.Find(_licenseId);
            if (currentLicense == null)
            {
                MessageBox.Show("Selected license was not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Business.Application oldApplication = Business.Application.FindByApplicationId(currentLicense.ApplicationID);
            if (oldApplication == null)
            {
                MessageBox.Show("Original application for selected license was not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to release this detained license?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            decimal applicationFees = ApplicationType.GetApplicationTypeFees(
                ApplicationType.ApplicationTypeTitle.ReleaseDetainedLicense);

            // 1) Create release application
            Business.Application releaseApplication = new Business.Application()
            {
                PersonId = oldApplication.PersonId,
                ApplicationDate = DateTime.Now,
                ApplicationTypeId = (int)ApplicationType.ApplicationTypeTitle.ReleaseDetainedLicense,
                ApplicationStatus = Business.Application.Status.New,
                LastStatusDate = DateTime.Now,
                PaidFees = applicationFees,
                UserId = AppSession.CurrentUserId
            };

            if (!releaseApplication.Save())
            {
                MessageBox.Show("Failed to create release application.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) Update detained license record
            _detainedLicense.IsReleased = true;
            _detainedLicense.ReleaseDate = DateTime.Now;
            _detainedLicense.ReleasedByUserID = AppSession.CurrentUserId;
            _detainedLicense.ReleaseApplicationID = releaseApplication.ApplicationId;

            if (!_detainedLicense.Save())
            {
                Business.Application.DeleteApplication(releaseApplication.ApplicationId);

                MessageBox.Show("Failed to release detained license.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3) Mark application as completed
            releaseApplication.ApplicationStatus = Business.Application.Status.Completed;
            releaseApplication.LastStatusDate = DateTime.Now;
            releaseApplication.Save();

            // 4) UI updates
            lblApplicationId.Text = releaseApplication.ApplicationId.ToString();
            lblTotalFees.Text = (applicationFees + _detainedLicense.FineFees).ToString();
            llbShowReleasedLicenseInfo.Enabled = true;
            btnRelease.Enabled = false;
            drivingLicenseInformationWithFilterControl1.LicenseFilterGroupBox.Enabled = false;
            drivingLicenseInformationWithFilterControl1.SetLicenseID = _licenseId.ToString();

            MessageBox.Show("Detained license released successfully.", "Released",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public int SelectedLicenseId
        {
            set
            {
                _licenseId = value;
                drivingLicenseInformationWithFilterControl1.TrySelectLicense(value);
            }
        }
    }
}
