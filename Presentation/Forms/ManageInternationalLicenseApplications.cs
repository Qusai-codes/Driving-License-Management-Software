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

namespace Presentation.Forms
{
    public partial class ManageInternationalLicenseApplications : Form
    {
        private DataTable _allInternationalLicenses = null;

        public ManageInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void ManageInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            SetupFilterComboBox();
            SetupIsActiveFilterComboBox();
            SetUpDataGridView();
        }

        private void SetupFilterComboBox()
        {
            cmbFilter.Items.Clear();
            string[] personFilterOptions = new string[] {
                "None", "Int. License ID", "Application ID", "Driver ID",
                "Local License ID", "Issue Date", "Expiration Date", "Is Active"
            };
            cmbFilter.Items.AddRange(personFilterOptions);
            cmbFilter.SelectedIndex = 0;
        }

        private void SetupIsActiveFilterComboBox()
        {
            cmbIsActiveFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIsActiveFilter.Items.AddRange(new object[] { "Yes", "No" });
            cmbIsActiveFilter.SelectedIndex = 0;

            // setting the size and location of is active combo box to 
            // same of that of the filter value text box.
            cmbIsActiveFilter.Width = txtFilterValue.Width;
            cmbIsActiveFilter.Height = txtFilterValue.Height;
            cmbIsActiveFilter.Left = txtFilterValue.Left;
            cmbIsActiveFilter.Top = txtFilterValue.Top;
            cmbIsActiveFilter.Visible = false;
        }

        private void SetUpDataGridView()
        {
            try
            {
                _allInternationalLicenses = GetInternationalDrivingLicenses();

                dgvInternationalLicenseApplications.AutoGenerateColumns = true;
                dgvInternationalLicenseApplications.DataSource = _allInternationalLicenses;

                FormatDataGridView();
                ApplyFilter();
                lblNumberOfRecords.Text = dgvInternationalLicenseApplications.Rows.Count.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error loading international license applications: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            SetColumnHeader(dgvInternationalLicenseApplications, "InternationalLicenseID", "Int. License ID");
            SetColumnHeader(dgvInternationalLicenseApplications, "ApplicationID", "Application ID");
            SetColumnHeader(dgvInternationalLicenseApplications, "DriverID", "Driver ID");
            SetColumnHeader(dgvInternationalLicenseApplications, "IssuedUsingLocalLicenseID", "Local License ID");
            SetColumnHeader(dgvInternationalLicenseApplications, "IssueDate", "Issue Date");
            SetColumnHeader(dgvInternationalLicenseApplications, "ExpirationDate", "Expiration Date");
            SetColumnHeader(dgvInternationalLicenseApplications, "IsActive", "Is Active");

            dgvInternationalLicenseApplications.Columns["CreatedByUserID"].Visible = false;
        }

        private void RefreshList()
        {
            SetUpDataGridView();
        }

        private DataTable GetInternationalDrivingLicenses()
        {
            return InternationalLicense.GetAllInternationalDrivingLicenses();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbFilter.SelectedItem as string;

            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
                cmbIsActiveFilter.Visible = false;
            }
            else if (selected == "Is Active")
            {
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
                cmbIsActiveFilter.Visible = true;
            }
            else
            {
                cmbIsActiveFilter.Visible = false;
                txtFilterValue.Visible = true;
                txtFilterValue.Clear();
            }

            ApplyFilter();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            NewInternationalLicenseApplicationForm form = new NewInternationalLicenseApplicationForm();
            form.ShowDialog();
            RefreshList();
        }

        private void ApplyFilter()
        {
            if (_allInternationalLicenses == null)
                return;

            DataView dv = _allInternationalLicenses.DefaultView;
            string selected = cmbFilter.SelectedItem as string;

            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            if (selected == "Is Active")
            {
                if (cmbIsActiveFilter == null || cmbIsActiveFilter.SelectedItem == null)
                {
                    dv.RowFilter = "";
                }
                else
                {
                    bool isActive = cmbIsActiveFilter.SelectedItem.ToString() == "Yes";
                    dv.RowFilter = string.Format("IsActive = {0}", isActive ? "true" : "false");
                }

                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            string filterText = txtFilterValue.Text.Trim();
            if (string.IsNullOrEmpty(filterText))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            string column = GetColumnNameFromDisplayName(selected);
            if (string.IsNullOrEmpty(column) || !_allInternationalLicenses.Columns.Contains(column))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            Type colType = _allInternationalLicenses.Columns[column].DataType;

            if (colType == typeof(string))
            {
                dv.RowFilter = string.Format("{0} LIKE '%{1}%'", column, filterText.Replace("'", "''"));
            }
            else if (colType == typeof(int))
            {
                int number;
                dv.RowFilter = int.TryParse(filterText, out number)
                    ? string.Format("{0} = {1}", column, number)
                    : "1=0";
            }
            else if (colType == typeof(DateTime))
            {
                dv.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '%{1}%'", column, filterText.Replace("'", "''"));
            }
            else
            {
                dv.RowFilter = "";
            }

            lblNumberOfRecords.Text = dv.Count.ToString();
        }

        private string GetColumnNameFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "Int. License ID":
                    return "InternationalLicenseID";
                case "Application ID":
                    return "ApplicationID";
                case "Driver ID":
                    return "DriverID";
                case "Local License ID":
                    return "IssuedUsingLocalLicenseID";
                case "Issue Date":
                    return "IssueDate";
                case "Expiration Date":
                    return "ExpirationDate";
                case "Is Active":
                    return "IsActive";
                default:
                    return null;
            }
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInternationalLicenseApplications.CurrentRow == null)
                return;

            int driverId = (int)dgvInternationalLicenseApplications.CurrentRow.Cells["DriverID"].Value;
            Driver driver = Driver.FindByDriverId(driverId);

            if (driver == null)
            {
                MessageBox.Show("Driver not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PersonDetailsForm form = new PersonDetailsForm(driver.PersonId);
            form.ShowDialog();
            RefreshList();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInternationalLicenseApplications.CurrentRow == null)
                return;

            int internationalLicenseId =
                (int)dgvInternationalLicenseApplications.CurrentRow.Cells["InternationalLicenseID"].Value;

            InternationalDrivingLicenseInformation form =
                new InternationalDrivingLicenseInformation(internationalLicenseId);

            form.ShowDialog();
            RefreshList();
        }

        private void showPersonLicenseHIstoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInternationalLicenseApplications.CurrentRow == null)
                return;

            int driverId = (int)dgvInternationalLicenseApplications.CurrentRow.Cells["DriverID"].Value;

            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(driverId);
            form.ShowDialog();
            RefreshList();
        }

        private void SetColumnHeader(DataGridView dgv, string columnName, string headerText)
        {
            if (dgv.Columns[columnName] != null)
            {
                dgv.Columns[columnName].HeaderText = headerText;
            }
        }

        private void cmbIsActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}
