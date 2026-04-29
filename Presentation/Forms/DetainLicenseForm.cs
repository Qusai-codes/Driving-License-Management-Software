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
    public partial class DetainLicenseForm : Form
    {
        private int _licenseId;

        public DetainLicenseForm()
        {
            InitializeComponent();
            drivingLicenseInformationWithFilterControl1.LicenseSelected += DrivingLicenseInformationWithFilterControl1_LicenseSelected;
        }

        private void DetainLicenseForm_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblCreatedByUser.Text = AppSession.CurrentUser.UserName;

            llbShowLicensesHistory.Enabled = false;
            llbLicenseInfo.Enabled = false;
            btnDetain.Enabled = false;
        }

        private void DrivingLicenseInformationWithFilterControl1_LicenseSelected(object sender, Events.LicenseSelectedEventArgs e)
        {
            _licenseId = e.LicenseId;

            Business.License currentLicense = Business.License.Find(_licenseId);
            if (currentLicense == null)
            {
                return;
            }

            // Rules checking
            // Can not detain expired license
            if (currentLicense.ExpirationDate.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "Selected license is expired.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Can not detain inactive license
            if (currentLicense.IsActive == false)
            {
                MessageBox.Show(
                    "Selected license is inactive.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Can not detain an already detained license
            if (Business.DetainedLicense.IsLicenseDetained(_licenseId))
            {
                MessageBox.Show("Selected license is already detained, choose another one.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update UI
            lblLicenseId.Text = _licenseId.ToString();
            btnDetain.Enabled = true;
            llbShowLicensesHistory.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (_licenseId <= 0)
            {
                MessageBox.Show("Please select a valid license first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtFineFees.Text, out decimal fineFees) || fineFees < 0)
            {
                MessageBox.Show("Please enter a valid fine fee (zero or a positive number).", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to detain this license?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }
            
            Business.DetainedLicense detainedLicense = new Business.DetainedLicense()
            {
                LicenseId = _licenseId,
                DetainDate = DateTime.Now,
                FineFees = fineFees,
                CreatedByUserID = AppSession.CurrentUserId,
                IsReleased = false,
                ReleaseDate = DateTime.MinValue,
                ReleasedByUserID = -1,
                ReleaseApplicationID = -1
            };

            if (!detainedLicense.Save())
            {
                MessageBox.Show("Failed to detain the license.", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblDetainId.Text = detainedLicense.DetainId.ToString();

            btnDetain.Enabled = false;
            drivingLicenseInformationWithFilterControl1.LicenseFilterGroupBox.Enabled = false;
            llbLicenseInfo.Enabled = true;
            drivingLicenseInformationWithFilterControl1.SetLicenseID = _licenseId.ToString();

            MessageBox.Show("License detained successfully with ID = " + detainedLicense.DetainId,
                "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void llbShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Business.License currentLicense = Business.License.Find(_licenseId);
            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(currentLicense.DriverID);
            form.ShowDialog();
        }

        private void llbLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInformationForm form = new DriverLicenseInformationForm(_licenseId);
            form.ShowDialog();
        }

        private void txtFineFees_TextChanged(object sender, EventArgs e)
        {
            string fineFeesValue = txtFineFees.Text;
            decimal fineFee = 0;
            if (!decimal.TryParse(fineFeesValue, out fineFee) || fineFee < 0)
            {
                errorProvider1.SetError(txtFineFees, "Cannot enter non-numeric values and " +
                    "the fine must be positive number or zero");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, "");
            }
        }
    }
}
