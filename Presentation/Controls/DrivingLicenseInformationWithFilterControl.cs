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

namespace Presentation.Controls
{
    public partial class DrivingLicenseInformationWithFilterControl : UserControl
    {
        private int _licenseId = -1;

        public event EventHandler<LicenseSelectedEventArgs> LicenseSelected;

        public DrivingLicenseInformationWithFilterControl()
        {
            InitializeComponent();
        }

        private void btnFindLicense_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtDrivingLicenseId.Text, out int licenseId))
            {
                return;
            }

            TryLoadLicense(licenseId, true);
        }

        public GroupBox LicenseFilterGroupBox
        {
            get
            {
                return grbFilter;
            }
        }

        public string SetLicenseID
        {
            set
            {
                txtDrivingLicenseId.Text = value;

                if (int.TryParse(value, out int licenseId))
                {
                    TryLoadLicense(licenseId, false);
                }
            }
        }

        protected virtual void OnLicenseSelected(int licenseId)
        {
            LicenseSelected?.Invoke(this, new LicenseSelectedEventArgs(licenseId));
        }

        private void txtDrivingLicenseId_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isControl = char.IsControl(e.KeyChar);
            bool isDigit = char.IsDigit(e.KeyChar);

            e.Handled = !(isControl || isDigit);
        }

        private void txtDrivingLicenseId_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (!tb.Text.All(char.IsDigit))
            {
                tb.Text = new string(tb.Text.Where(char.IsDigit).ToArray());
                tb.SelectionStart = tb.Text.Length;
            }
        }

        private bool VerifyLicenseId(int licenseId)
        {
            return Business.License.DoesLicenseExistByLicenseId(licenseId);
        }

        public void TrySelectLicense(int licenseId)
        {
            TryLoadLicense(licenseId, false);
        }

        private bool TryLoadLicense(int licenseId, bool showNotFoundMessage)
        {
            if (!VerifyLicenseId(licenseId))
            {
                if (showNotFoundMessage)
                {
                    string message = string.Format("There is no driving license with id = {0}", licenseId);
                    MessageBox.Show(message, "License Does Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }

            _licenseId = licenseId;

            if (txtDrivingLicenseId.Text != licenseId.ToString())
            {
                txtDrivingLicenseId.Text = licenseId.ToString();
            }

            int driverId = Business.License.GetDriverIdByLicenseId(licenseId);
            if (driverId == -1)
            {
                if (showNotFoundMessage)
                {
                    MessageBox.Show("Driver not found for selected license.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }

            drivingLicenseInformationControl1.DriverId = driverId;
            drivingLicenseInformationControl1.LicenseId = _licenseId;

            OnLicenseSelected(_licenseId);

            return true;
        }
    }
}
