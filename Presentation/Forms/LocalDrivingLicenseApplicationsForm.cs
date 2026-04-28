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

        public LocalDrivingLicenseApplicationsForm()
        {
            InitializeComponent();
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

            Type colType = _allLocalDrivingLicenseApplications.Columns[column].DataType;

            // Special handling for status (stored as numeric, displayed as enum text in grid formatting only)
            if (selected == "Status")
            {
                byte statusValue;
                if (byte.TryParse(filterText, out statusValue))
                {
                    dv.RowFilter = string.Format("{0} = {1}", column, statusValue);
                }
                else
                {
                    Business.Application.Status statusEnum;
                    if (Enum.TryParse(filterText, true, out statusEnum))
                    {
                        dv.RowFilter = string.Format("{0} = {1}", column, (byte)statusEnum);
                    }
                    else
                    {
                        dv.RowFilter = "1=0";
                    }
                }

                lblNumberOfRecords.Text = dv.Count.ToString();
                return;
            }

            if (colType == typeof(string))
            {
                dv.RowFilter = string.Format("{0} LIKE '%{1}%'", column, filterText.Replace("'", "''"));
            }
            else if (colType == typeof(int) || colType == typeof(byte) || colType == typeof(short))
            {
                int num;
                if (int.TryParse(filterText, out num))
                    dv.RowFilter = string.Format("{0} = {1}", column, num);
                else
                    dv.RowFilter = "1=0";
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

            if (dgvApplications.CurrentRow == null)
            {
                return;
            }

            int drivingLicenseApplicationId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            IssueDrivingLicenseForFirstTimeForm form = new IssueDrivingLicenseForFirstTimeForm(drivingLicenseApplicationId);
            form.ShowDialog();
            RefreshApplicationsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow == null)
            {
                return;
            }

            int localAppId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            int appId = LocalDrivingLicenseApplication.GetApplicationId(localAppId);

            if (appId == -1)
            {
                MessageBox.Show("Application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int licenseId = Business.License.GetLicenseIdByApplicationId(appId);
            if (licenseId == -1)
            {
                MessageBox.Show("No issued license found for this application.", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DriverLicenseInformationForm form = new DriverLicenseInformationForm(licenseId);
            form.ShowDialog();
            RefreshApplicationsList();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow == null)
            {
                return;
            }
            string personNationalNumber = (string)dgvApplications.CurrentRow.Cells["NationalNo"].Value;
            Person person = Person.Find(personNationalNumber);
            int driverId = Driver.GetDriverIdByPersonId(person.PersonId);
            DriverLicensesHistoryForm form = new DriverLicensesHistoryForm(driverId);
            form.ShowDialog();
            RefreshApplicationsList();

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

            int localAppId = (int)dgvApplications.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            int appId = LocalDrivingLicenseApplication.GetApplicationId(localAppId);

            object statusObj = dgvApplications.CurrentRow.Cells["ApplicationStatus"].Value;
            object passedTestsObj = dgvApplications.CurrentRow.Cells["PassedTests"].Value;

            byte statusValue = 0;
            int passedTests = 0;

            if (statusObj != null)
                byte.TryParse(statusObj.ToString(), out statusValue);

            if (passedTestsObj != null)
                int.TryParse(passedTestsObj.ToString(), out passedTests);

            bool isNewStatus = statusValue == (byte)Business.Application.Status.New;
            int totalTests = TestType.GetTestTypeCount();

            bool hasLicense = appId > 0 && Business.License.DoesLicenseExistByApplicationId(appId);

            // Show License availability
            showLicenseToolStripMenuItem.Enabled = hasLicense;

            // Disable menu items related to edition of the local driving license application
            // if the license is already issued.
            editApplicationToolStripMenuItem.Enabled = !hasLicense;
            deleteApplicationToolStripMenuItem.Enabled = !hasLicense;
            cancelApplicationToolStripMenuItem.Enabled = !hasLicense;

            // If not New status, scheduling/issuing first time is not allowed
            if (!isNewStatus)
            {
                scheduleTestsToolStripMenuItem.Enabled = false;
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                return;
            }

            // Issue first time only when all tests passed and no license exists yet
            bool canIssueFirstTime = (passedTests >= totalTests) && !hasLicense;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = canIssueFirstTime;

            // If license already exists OR can issue first time, disable scheduling
            if (hasLicense || canIssueFirstTime)
            {
                scheduleTestsToolStripMenuItem.Enabled = false;
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
                return;
            }

            // Scheduling rules
            scheduleTestsToolStripMenuItem.Enabled = true;

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
            else
            {
                // Safety fallback
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }

        }
    }
}
