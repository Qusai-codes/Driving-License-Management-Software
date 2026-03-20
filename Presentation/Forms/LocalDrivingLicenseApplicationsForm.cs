using Business;
using Contracts.DTOs;
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
    public partial class LocalDrivingLicenseApplicationsForm : Form
    {
        private DataTable _allLocalDrivingLicenseApplications = null;
        private readonly int _currentUserId;

        public LocalDrivingLicenseApplicationsForm(int currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
        }

        private void LocalDrivingLicenseApplicationsForm_Load(object sender, EventArgs e)
        {
            SetupFilterComboBox();
            RefreshApplicationsList();
            txtFilterValue.Visible = false;
        }

        private DataTable GetLocalDrivingLicenseApplications()
        {
            return LocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
        }

        private void RefreshApplicationsList()
        {
            try
            {
                _allLocalDrivingLicenseApplications = GetLocalDrivingLicenseApplications();

                dgvApplications.AutoGenerateColumns = true;
                dgvApplications.DataSource = _allLocalDrivingLicenseApplications;

                FormatDataGridView();
                ApplyFilter();
                lblNumberOfRecords.Text = dgvApplications.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applications: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Setting column headers of data grid view
            SetColumnHeader("LocalDrivingLicenseApplicationID", "L.D.L.AppID");
            SetColumnHeader("ClassName", "Driving Class");
            SetColumnHeader("NationalNo", "National No.");
            SetColumnHeader("FullName", "Full Name");
            SetColumnHeader("ApplicationDate", "Application Date");
            SetColumnHeader("PassedTests", "Passed Tests");
            SetColumnHeader("ApplicationStatus", "Status");

            // Transform ApplicationStatus display from number to enum name
            dgvApplications.CellFormatting -= dgvApplications_CellFormatting;
            dgvApplications.CellFormatting += dgvApplications_CellFormatting;

        }

        private void dgvApplications_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvApplications.Columns[e.ColumnIndex].Name != "ApplicationStatus" || e.Value == null)
                return;

            byte statusValue;
            if (!byte.TryParse(e.Value.ToString(), out statusValue))
                return;

            if (Enum.IsDefined(typeof(Business.Application.Status), (int)statusValue))
            {
                e.Value = ((Business.Application.Status)statusValue).ToString();
            }
            else
            {
                e.Value = "Unknown";
            }

            e.FormattingApplied = true;
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvApplications.Columns[columnName] != null)
            {
                dgvApplications.Columns[columnName].HeaderText = headerText;
            }
        }

        private void SetupFilterComboBox()
        {
            cmbFilterApplications.Items.Clear();
            string[] applicationsFilterOption = new string[] {
                "None", "L.D.L.AppID", "National No.", "Full Name",
                "Status"
            };
            cmbFilterApplications.Items.AddRange(applicationsFilterOption);
            cmbFilterApplications.SelectedIndex = 0;
        }

        private void cmbFilterApplications_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterApplications.SelectedItem == null)
            {
                return;
            }

            string selectedFilter = cmbFilterApplications.SelectedItem.ToString();

            if (selectedFilter == "None")
            {
                // Hide textbox and show all data
                txtFilterValue.Visible = false;
                txtFilterValue.Clear();
                ApplyFilter();
            }
            else
            {
                // Show textbox for filtering
                txtFilterValue.Visible = true;
                txtFilterValue.Clear();
            }
        }

        private void ApplyFilter()
        {
            if (_allLocalDrivingLicenseApplications == null)
                return;

            DataView dv = _allLocalDrivingLicenseApplications.DefaultView;

            string selected = cmbFilterApplications.SelectedItem as string;
            string filterText = txtFilterValue.Text.Trim();

            // Reset filter if "None"
            if (string.IsNullOrEmpty(selected) || selected == "None")
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            if (string.IsNullOrEmpty(filterText))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            string column = GetColumnNameFromDisplayName(selected);

            if (string.IsNullOrEmpty(column) || !_allLocalDrivingLicenseApplications.Columns.Contains(column))
            {
                dv.RowFilter = "";
                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            // Build filter based on column type
            Type colType = _allLocalDrivingLicenseApplications.Columns[column].DataType;

            if (colType == typeof(string))
            {
                // safe, correct SQL‑style filter expression
                dv.RowFilter = $"{column} LIKE '%{filterText.Replace("'", "''")}%'";
            }
            else if (colType == typeof(int))
            {
                if (int.TryParse(filterText, out int num))
                    dv.RowFilter = $"{column} = {num}";
                else
                    dv.RowFilter = "1=0";
            }

            lblNumberOfRecords.Text = dv.Count.ToString();
        }

        private string GetColumnNameFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "L.D.L.AppID":
                    return "LocalDrivingLicenseApplicationID";
                case "Driving Class":
                    return "ClassName";
                case "National No.":
                    return "NationalNo";
                case "Full Name":
                    return "FullName";
                case "Application Date":
                    return "ApplicationDate";
                case "Passed Tests":
                    return "PassedTests";
                case "Status":
                    return "ApplicationStatus";
                default:
                    return null;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {

            LocalDrivingLicenseForm form = new LocalDrivingLicenseForm(FormMode.Add, _currentUserId);
            form.ShowDialog();
            RefreshApplicationsList();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel this application?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // Cancel the application

                // Get application id 
                if (dgvApplications.CurrentRow == null)
                {
                    MessageBox.Show("Please select an application");
                    return;
                }
                int localApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
                bool cancelled = LocalDrivingLicenseApplication.CancelApplication(localApplicationId);
                if (!cancelled)
                {
                    MessageBox.Show("Unable to cancel the application", "Fail",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Application Cancelled Successfully.",
                    "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshApplicationsList();
            }
            
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow == null)
            {
                return;
            }
            int drivingLicenseApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            VisionTestAppointmentForm form = new VisionTestAppointmentForm(drivingLicenseApplicationId);
            form.ShowDialog();

        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Complete the implementation.

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Complete the implementation.
        }
    }
}
