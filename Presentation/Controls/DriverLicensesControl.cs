using Business;
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
    public partial class DriverLicensesControl : UserControl
    {
        

        public DriverLicensesControl()
        {
            InitializeComponent();
        }

        public int DriverId
        {
            set
            {
                int driverId = value;
                LoadData(driverId);
            }
        }

        private void LoadData(int driverId)
        {
            LoadLocalLicensesData(driverId);
            LoadInternationalLicenseData(driverId);
        }

        private void LoadInternationalLicenseData(int driverId)
        {
            try
            {
                DataTable dt = InternationalLicense.GetInternationalDrivingLicense(driverId);

                dgvInternationalDrivingLicenses.AutoGenerateColumns = true;
                dgvInternationalDrivingLicenses.DataSource = dt;

                SetColumnHeader(dgvInternationalDrivingLicenses, "InternationalLicenseID", "Int. License ID");
                SetColumnHeader(dgvInternationalDrivingLicenses, "ApplicationID", "Application ID");
                SetColumnHeader(dgvInternationalDrivingLicenses, "IssuedUsingLocalLicenseID", "L. License ID");
                SetColumnHeader(dgvInternationalDrivingLicenses, "IssueDate", "Issue Date");
                SetColumnHeader(dgvInternationalDrivingLicenses, "ExpirationDate", "Expiration Date");
                SetColumnHeader(dgvInternationalDrivingLicenses, "IsActive", "Is Active");

                lblNumberOfInternationalLicenseRecords.Text =
                    (dt != null ? dt.Rows.Count : 0).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading international licenses: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblNumberOfInternationalLicenseRecords.Text = "0";
            }
        }

        private void LoadLocalLicensesData(int driverId)
        {
            try
            {
                DataTable dt = Business.License.GetAllLocalLicenses(driverId);

                dgvLocalDrivingLicenses.AutoGenerateColumns = true;
                dgvLocalDrivingLicenses.DataSource = dt;

                SetColumnHeader(dgvLocalDrivingLicenses, "LicenseID", "License ID");
                SetColumnHeader(dgvLocalDrivingLicenses, "ApplicationID", "Application ID");
                SetColumnHeader(dgvLocalDrivingLicenses, "ClassName", "Class Name");
                SetColumnHeader(dgvLocalDrivingLicenses, "IssueDate", "Issue Date");
                SetColumnHeader(dgvLocalDrivingLicenses, "ExpirationDate", "Expiration Date");
                SetColumnHeader(dgvLocalDrivingLicenses, "IsActive", "Is Active");

                lblNumberOfLocalLicenseRecords.Text =
                    (dt != null ? dt.Rows.Count : 0).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading local licenses: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblNumberOfLocalLicenseRecords.Text = "0";
            }
        }

        private void SetColumnHeader(DataGridView dgv, string columnName, string headerText)
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName].HeaderText = headerText;
            }
        }
    }
}
