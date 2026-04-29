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
            if (int.TryParse(txtDrivingLicenseId.Text, out int result))
            {
                int licenseId = result;
                if (!VerifyLicenseId(licenseId))
                {
                    string message = string.Format("There is no driving license with id = {0}",
                        licenseId);
                    MessageBox.Show(message, "License Does Not Exist", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                _licenseId = licenseId;
                OnLicenseSelected(_licenseId);

                // Call the setter for child control
                int driverId = Business.License.GetDriverIdByLicenseId(licenseId);
                if (driverId != -1)
                {
                    drivingLicenseInformationControl1.DriverId = driverId;
                }
                else
                {
                    return;
                }

                drivingLicenseInformationControl1.LicenseId = _licenseId;
            }
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
            if (!VerifyLicenseId(licenseId))
                return;

            _licenseId = licenseId;
            txtDrivingLicenseId.Text = licenseId.ToString();

            OnLicenseSelected(_licenseId);

            // Call the setter for child control
            int driverId = Business.License.GetDriverIdByLicenseId(licenseId);
            if (driverId != -1)
            {
                drivingLicenseInformationControl1.DriverId = driverId;
            }
            else
            {
                return;
            }

            drivingLicenseInformationControl1.LicenseId = _licenseId;

            return;
        }
    }
}
