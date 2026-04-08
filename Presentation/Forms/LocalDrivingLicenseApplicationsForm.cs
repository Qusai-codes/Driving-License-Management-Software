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

            LocalDrivingLicenseForm form = new LocalDrivingLicenseForm(-1, FormMode.Add);
            form.ShowDialog();
            RefreshApplicationsList();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow != null)
            {
                int localApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
                DrivingLicenseApplicationInformationForm form = new DrivingLicenseApplicationInformationForm(localApplicationId);
                form.ShowDialog();
                RefreshApplicationsList();
            }
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow != null)
            {
                int localApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
                LocalDrivingLicenseForm form = new LocalDrivingLicenseForm(localApplicationId, FormMode.Edit);
                form.ShowDialog();
                RefreshApplicationsList();
            }
            
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this application?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // Delete the application

                // Get application id 
                if (dgvApplications.CurrentRow == null)
                {
                    MessageBox.Show("Please select an application");
                    return;
                }
                int localApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
                bool deleted = LocalDrivingLicenseApplication.DeleteApplication(localApplicationId);
                if (!deleted)
                {
                    MessageBox.Show("Unable to delete the application", "Fail",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Application Deleted Successfully.",
                    "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshApplicationsList();
            }
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
            // TODO: implement the functionality.
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: implement the functionality.
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: implement the functionality.
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow == null)
            {
                return;
            }
            int drivingLicenseApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            TestAppointmentForm form = new TestAppointmentForm(drivingLicenseApplicationId, TestType.TestTypeId.Vision);
            form.ShowDialog();
            RefreshApplicationsList();

        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow == null)
            {
                return;
            }
            int drivingLicenseApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            TestAppointmentForm form = new TestAppointmentForm(drivingLicenseApplicationId, TestType.TestTypeId.Written);
            form.ShowDialog();
            RefreshApplicationsList();

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow == null)
            {
                return;
            }
            int drivingLicenseApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            TestAppointmentForm form = new TestAppointmentForm(drivingLicenseApplicationId, TestType.TestTypeId.Street);
            form.ShowDialog();
            RefreshApplicationsList();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ApplyScheduleTestsMenuRules();
        }

        private void ApplyScheduleTestsMenuRules()
        {
            if (dgvApplications.CurrentRow == null)
            {
                return;
            }

            object statusObj = dgvApplications.CurrentRow.Cells["ApplicationStatus"].Value;
            object passedTestsObj = dgvApplications.CurrentRow.Cells["PassedTests"].Value;

            byte statusValue = 0;
            int passedTests = 0;

            if (statusObj != null)
                byte.TryParse(statusObj.ToString(), out statusValue);

            if (passedTestsObj != null)
                int.TryParse(passedTestsObj.ToString(), out passedTests);

            // Scheduling is allowed only for "New" applications.
            bool isNewStatus = statusValue == (byte)Business.Application.Status.New;

            if (!isNewStatus)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
                return;
            }

            // Issuing driving license for first time
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;

            // Rules:
            // 1) If vision not passed yet -> disable written + street.
            // 2) If vision passed -> enable only written (street stays disabled).
            // 3) If both vision and written passed -> allow scheduling of street test.
            if (passedTests <= 0)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = true;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }
            else if (passedTests == 1)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = true;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }
            else if (passedTests == 2)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = true;
            }
            else if (passedTests == 3)
            {
                scheduleTestsToolStripMenuItem.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
            }
        }
    }
}
