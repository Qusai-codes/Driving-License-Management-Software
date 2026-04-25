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
    public partial class RenewDrivingLicenseApplication : Form
    {
        private int _localLicenseId = -1;
        private int _renewedLicenseId = -1;

        public RenewDrivingLicenseApplication()
        {
            InitializeComponent();
            drivingLicenseInformationWithFilterControl1.LicenseSelected += DrivingLicenseInformationWithFilterControl1_LicenseSelected;
        }

        private void RenewDrivingLicenseApplication_Load(object sender, EventArgs e)
        {

            lblApplicationDate.Text = DateTime.Now.ToString("d");
            lblIssueDate.Text = DateTime.Now.ToString("d");

            Business.User user = Business.User.Find(Helpers.AppSession.CurrentUserId);
            lblCreatedByUserName.Text = user.UserName;
            lblApplicationFees.Text = Business.ApplicationType.GetApplicationTypeFees(
                Business.ApplicationType.ApplicationTypeTitle.RenewDrivingLicense).ToString();

            llbShowLicensesHistory.Enabled = false;
            llbShowNewLicenseInfo.Enabled = false;
            btnRenew.Enabled = false;

        }

        private void DrivingLicenseInformationWithFilterControl1_LicenseSelected(object sender, LicenseSelectedEventArgs e)
        {
            _localLicenseId = e.LicenseId;
            Business.License currentLicense = Business.License.Find(_localLicenseId);

            llbShowLicensesHistory.Enabled = true;
            lblOldLicenseId.Text = currentLicense.LicenseID.ToString();
            DateTime expirationDateForNewLicense = DateTime.Now.AddYears((int)LicenseClass.GetDefaultValidityLength(
                currentLicense.LicenseID));
            lblExpirationDate.Text = expirationDateForNewLicense.ToString("d");
            lblLicenseFees.Text = LicenseClass.GetLicenseClassFees(currentLicense.LicenseClass).ToString();
            lblTotalFees.Text = CalculateTotalFees().ToString();


            if (currentLicense.IsActive == false)
            {
                MessageBox.Show("Selected license is not active. Only active licenses can be renewed.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Not expired yet => renewal is not allowed.
            if (currentLicense.ExpirationDate.Date >= DateTime.Today)
            {
                MessageBox.Show(
                    "Selected license is not expired yet. It will expire on: " +
                    currentLicense.ExpirationDate.ToString("d"),
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnRenew.Enabled = true;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (_localLicenseId <= 0)
            {
                MessageBox.Show("Please select a valid license first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Business.License currentLicense = Business.License.Find(_localLicenseId);
            if (currentLicense == null)
            {
                MessageBox.Show("Selected license was not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!currentLicense.IsActive)
            {
                MessageBox.Show("Selected license is not active. Only active licenses can be renewed.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentLicense.ExpirationDate.Date >= DateTime.Today)
            {
                MessageBox.Show(
                    "Selected license is not expired yet. It will expire on: " +
                    currentLicense.ExpirationDate.ToString("d"),
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Business.Application oldApplication = Business.Application.FindByApplicationId(currentLicense.ApplicationID);
            if (oldApplication == null)
            {
                MessageBox.Show("Original application for selected license was not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dlg = MessageBox.Show("Are you sure you want to renew the license?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dlg != DialogResult.Yes)
                return;

            decimal renewalApplicationFees = ApplicationType.GetApplicationTypeFees(
                ApplicationType.ApplicationTypeTitle.RenewDrivingLicense);

            decimal renewedLicenseFees = LicenseClass.GetLicenseClassFees(currentLicense.LicenseClass);

            // 1) Create renewal application
            Business.Application renewalApplication = new Business.Application()
            {
                PersonId = oldApplication.PersonId,
                ApplicationDate = DateTime.Now,
                ApplicationTypeId = (int)ApplicationType.ApplicationTypeTitle.RenewDrivingLicense,
                ApplicationStatus = Business.Application.Status.New,
                LastStatusDate = DateTime.Now,
                PaidFees = renewalApplicationFees,
                UserId = Helpers.AppSession.CurrentUserId
            };

            if (!renewalApplication.Save())
            {
                MessageBox.Show("Failed to create renewal application.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) Deactivate old license first
            currentLicense.IsActive = false;
            if (!currentLicense.Save())
            {
                Business.Application.DeleteApplication(renewalApplication.ApplicationId);

                MessageBox.Show("Failed to deactivate old license.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3) Create renewed license
            DateTime issueDate = DateTime.Now;

            Business.License renewedLicense = new Business.License()
            {
                ApplicationID = renewalApplication.ApplicationId,
                DriverID = currentLicense.DriverID,
                LicenseClass = currentLicense.LicenseClass,
                IssueDate = issueDate,
                ExpirationDate = issueDate.AddYears(LicenseClass.GetDefaultValidityLength(currentLicense.LicenseClass)),
                Notes = txtNotes.Text.Trim(),
                PaidFees = renewedLicenseFees,
                IsActive = true,
                IssueReason = (byte)Business.License.IssueReasonType.Renew,
                CreatedByUserID = Helpers.AppSession.CurrentUserId
            };

            if (!renewedLicense.Save())
            {
                // rollback best effort
                currentLicense.IsActive = true;
                currentLicense.Save();
                Business.Application.DeleteApplication(renewalApplication.ApplicationId);

                MessageBox.Show("Failed to issue renewed license.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _renewedLicenseId = renewedLicense.LicenseID;

            // 4) UI updates
            lblRenewLicenseApplicationId.Text = renewalApplication.ApplicationId.ToString();
            lblRenewedLicenseId.Text = renewedLicense.LicenseID.ToString();
            lblIssueDate.Text = renewedLicense.IssueDate.ToString("d");
            lblExpirationDate.Text = renewedLicense.ExpirationDate.ToString("d");
            lblApplicationFees.Text = renewalApplicationFees.ToString();
            lblLicenseFees.Text = renewedLicenseFees.ToString();
            lblTotalFees.Text = (renewalApplicationFees + renewedLicenseFees).ToString();

            llbShowNewLicenseInfo.Enabled = true;
            btnRenew.Enabled = false;
            drivingLicenseInformationWithFilterControl1.LicenseFilterGroupBox.Enabled = false;

            MessageBox.Show("License Renewed Successfully with ID = " + renewedLicense.LicenseID,
                "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void llbShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Business.License currentLicense = Business.License.Find(_localLicenseId);
            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(currentLicense.DriverID);
            form.ShowDialog();
        }

        private void llbShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInformationForm form = new DriverLicenseInformationForm(_renewedLicenseId);
            form.ShowDialog();
        }

        private decimal CalculateTotalFees()
        {
            decimal applicationFees = ApplicationType.GetApplicationTypeFees(
                ApplicationType.ApplicationTypeTitle.RenewDrivingLicense);

            if (_localLicenseId <= 0)
                return applicationFees;

            Business.License currentLicense = Business.License.Find(_localLicenseId);
            if (currentLicense == null)
                return applicationFees;

            decimal licenseFees = LicenseClass.GetLicenseClassFees(currentLicense.LicenseClass);

            return applicationFees + licenseFees;
        }
    }
}
