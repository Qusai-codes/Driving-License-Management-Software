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
    public partial class ReplaceLostOrDamagedDrivingLicenseForm : Form
    {
        private int _localLicenseId = -1;
        private int _newLicenseId = -1;

        public ReplaceLostOrDamagedDrivingLicenseForm()
        {
            InitializeComponent();
            drivingLicenseInformationWithFilterControl1.LicenseSelected += DrivingLicenseInformationWithFilterControl1_LicenseSelected;
        }

        private void DrivingLicenseInformationWithFilterControl1_LicenseSelected(object sender, LicenseSelectedEventArgs e)
        {
            _localLicenseId = e.LicenseId;
            Business.License currentLicense = Business.License.Find(_localLicenseId);

            llbShowLicensesHistory.Enabled = true;
            lblOldLicenseId.Text = currentLicense.LicenseID.ToString();

            if (currentLicense.IsActive == false)
            {
                MessageBox.Show("Selected license is not active. Choose an active license.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Expired license replacement is not allowed.
            if (currentLicense.ExpirationDate.Date >= DateTime.Today)
            {
                MessageBox.Show(
                    "Selected license is expired.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssueReplacement.Enabled = true;
        }

        private void ReplaceLostOrDamagedDrivingLicenseForm_Load(object sender, EventArgs e)
        {
            rdoDamagedLicense.Checked = true;
            SetReplacementReasonData();
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblCreatedByUserName.Text = Helpers.AppSession.CurrentUser.UserName;
            btnIssueReplacement.Enabled = false;
            llbShowLicensesHistory.Enabled = false;
            llbShowNewLicenseInfo.Enabled = false;
        }

        private void SetReplacementReasonData()
        {
            if (rdoDamagedLicense.Checked)
            {
                this.Text = "Replacement for Damaged License";
                lblFormTitle.Text = "Replacement for Damaged License";
                lblApplicationFees.Text = ApplicationType.GetApplicationTypeFees(
                    ApplicationType.ApplicationTypeTitle.ReplaceDamagedDrivingLicense).ToString();
            }
            else if (rdoLostLicense.Checked)
            {
                this.Text = "Replacement for Lost License";
                lblFormTitle.Text = "Replacement for Lost License";
                lblApplicationFees.Text = ApplicationType.GetApplicationTypeFees(
                    ApplicationType.ApplicationTypeTitle.ReplaceLostDrivingLicense).ToString();
            }
        }

        private void llbShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Business.License currentLicense = Business.License.Find(_localLicenseId);
            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(currentLicense.DriverID);
            form.ShowDialog();
        }

        private void llbShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInformationForm form = new DriverLicenseInformationForm(_newLicenseId);
            form.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
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

            Business.Application oldApplication = Business.Application.FindByApplicationId(currentLicense.ApplicationID);
            if (oldApplication == null)
            {
                MessageBox.Show("Original application for selected license was not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dlg = MessageBox.Show("Are you sure you want to issue a replacement for the license?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dlg != DialogResult.Yes)
                return;

            decimal replacementApplicationFees = rdoDamagedLicense.Checked 
                ?
                ApplicationType.GetApplicationTypeFees(
                    ApplicationType.ApplicationTypeTitle.ReplaceDamagedDrivingLicense)

                : ApplicationType.GetApplicationTypeFees(
                    ApplicationType.ApplicationTypeTitle.ReplaceLostDrivingLicense);

            decimal replacementOfLicenseFees = LicenseClass.GetLicenseClassFees(currentLicense.LicenseClass);

            // 1) Create license replacement application
            Business.Application licenseReplacementApplication = new Business.Application()
            {
                PersonId = oldApplication.PersonId,
                ApplicationDate = DateTime.Now,
                ApplicationTypeId = 
                rdoDamagedLicense.Checked ? (int)ApplicationType.ApplicationTypeTitle.ReplaceDamagedDrivingLicense
                                          : (int)ApplicationType.ApplicationTypeTitle.ReplaceLostDrivingLicense,
                ApplicationStatus = Business.Application.Status.New,
                LastStatusDate = DateTime.Now,
                PaidFees = replacementApplicationFees,
                UserId = Helpers.AppSession.CurrentUserId
            };

            if (!licenseReplacementApplication.Save())
            {
                MessageBox.Show("Failed to create replacement application.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) Deactivate old license first
            currentLicense.IsActive = false;
            if (!currentLicense.Save())
            {
                Business.Application.DeleteApplication(licenseReplacementApplication.ApplicationId);

                MessageBox.Show("Failed to deactivate old license.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3) Create renewed license
            DateTime issueDate = DateTime.Now;

            Business.License newLicense = new Business.License()
            {
                ApplicationID = licenseReplacementApplication.ApplicationId,
                DriverID = currentLicense.DriverID,
                LicenseClass = currentLicense.LicenseClass,
                IssueDate = issueDate,
                ExpirationDate = issueDate.AddYears(LicenseClass.GetDefaultValidityLength(currentLicense.LicenseClass)),
                Notes = string.Empty,
                PaidFees = replacementOfLicenseFees,
                IsActive = true,
                IssueReason = rdoDamagedLicense.Checked ? 
                    (byte)Business.License.IssueReasonType.ReplacementForDamaged 
                    : (byte)Business.License.IssueReasonType.ReplacementForLost,
                CreatedByUserID = Helpers.AppSession.CurrentUserId
            };

            if (!newLicense.Save())
            {
                // rollback best effort
                currentLicense.IsActive = true;
                currentLicense.Save();
                Business.Application.DeleteApplication(licenseReplacementApplication.ApplicationId);

                MessageBox.Show("Failed to issue renewed license.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _newLicenseId = newLicense.LicenseID;

            // 4) UI updates
            lblLicenseReplacementApplicationId.Text = licenseReplacementApplication.ApplicationId.ToString();
            lblReplacedLicenseId.Text = newLicense.LicenseID.ToString();
            lblApplicationFees.Text = replacementApplicationFees.ToString();

            llbShowNewLicenseInfo.Enabled = true;
            btnIssueReplacement.Enabled = false;
            drivingLicenseInformationWithFilterControl1.LicenseFilterGroupBox.Enabled = false;
            grpReplacementReason.Enabled = false;

            MessageBox.Show("License Replaced Successfully with ID = " + newLicense.LicenseID,
                "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void rdoDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            SetReplacementReasonData();
        }

        private void rdoLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            SetReplacementReasonData();
        }
    }
}
