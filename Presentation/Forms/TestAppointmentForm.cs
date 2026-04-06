using Business;
using Presentation.Helpers;
using Presentation.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Business.TestType;

namespace Presentation.Forms
{
    public partial class TestAppointmentForm : Form
    {
        private int _localDrivingLicenseApplicationId;
        private TestType.TestTypeId _testType;

        public TestAppointmentForm(int localDrivingLicenseApplicationId, 
            TestType.TestTypeId testType)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
            _testType = testType;
        }

        private void TestAppointmentForm_Load(object sender, EventArgs e)
        {
            drivingLicenseApplicationInformationControl1.LocalDrivingLicenseApplicationId =
                _localDrivingLicenseApplicationId;
            drivingLicenseApplicationInformationControl1.ShowLicenseInformationLinkLabel.Enabled = false;

            switch (_testType)
            {
                case TestType.TestTypeId.Vision:
                    picDrivingTestSymbol.Image = Resources.vision_test_symbol;
                    lblFormTitle.Text = "Vision Test Appointments";
                    break;
                case TestType.TestTypeId.Written:
                    picDrivingTestSymbol.Image = Resources.written_test_symbol;
                    lblFormTitle.Text = "Written Test Appointments";
                    break;
                case TestType.TestTypeId.Street:
                    picDrivingTestSymbol.Image = Resources.street_test_symbol;
                    lblFormTitle.Text = "Street Test Appointments";
                    break;
                default:
                    break;
            }

            RefreshAppointmentsList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool HasActiveUnlockedAppointment()
        {
            DataTable dt = TestAppointment.GetAllTestAppointments(_localDrivingLicenseApplicationId, _testType);
            if (dt == null || dt.Rows.Count == 0 || !dt.Columns.Contains("IsLocked"))
                return false;

            return dt.AsEnumerable().Any(r => !r.Field<bool>("IsLocked"));
        }

        private bool HasApplicantPassedTest()
        {
            int trials = TestAppointment.GetNumberOfTestTrials(_localDrivingLicenseApplicationId, _testType);
            if (trials == 0)
                return false;

            return TestAppointment.GetTestResult(_localDrivingLicenseApplicationId, _testType);
        }


        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            // 1) Existing open (not locked) appointment => block
            if (HasActiveUnlockedAppointment())
            {
                MessageBox.Show("Person already have an active appointment for this test, you "
                + "cannot add new appointment", "Not Allowed", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            // 2) Already passed this test => block
            if (HasApplicantPassedTest())
            {
                MessageBox.Show("This person already passed this test before, " +
                    "you can only retake failed test", "Not Allowed", MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            // 3) Allowed
            ScheduleDrivingTestForm form = new ScheduleDrivingTestForm(_localDrivingLicenseApplicationId,
                _testType, FormMode.Add, -1);
            form.ShowDialog();
            RefreshAppointmentsList();
        }

        private DataTable GetAllDrivingTypeTestAppointments()
        {
            return TestAppointment.GetAllTestAppointments(_localDrivingLicenseApplicationId, 
                _testType);
        }

        private void RefreshAppointmentsList()
        {
            try
            {
                DataTable testAppointments = GetAllDrivingTypeTestAppointments();

                dgvTestAppointments.AutoGenerateColumns = true;
                dgvTestAppointments.DataSource = testAppointments;

                FormatDataGridView();
                lblNumberOfRecords.Text = dgvTestAppointments.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading test appointments: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            SetColumnHeader("TestAppointmentID", "Appointment ID");
            SetColumnHeader("AppointmentDate", "Appointment Date");
            SetColumnHeader("PaidFees", "Paid Fees");
            SetColumnHeader("IsLocked", "Is Locked");
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvTestAppointments.Columns[columnName] != null)
            {
                dgvTestAppointments.Columns[columnName].HeaderText = headerText;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTestAppointments.CurrentRow != null)
            {
                int testAppointmentId = (int)dgvTestAppointments.CurrentRow.Cells["TestAppointmentID"].Value;
                ScheduleDrivingTestForm form = new ScheduleDrivingTestForm(_localDrivingLicenseApplicationId,
                _testType,FormMode.Edit, testAppointmentId);
                form.ShowDialog();
                RefreshAppointmentsList();
            }
            
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTestAppointments.CurrentRow != null)
            {
                int testAppointmentId = (int)dgvTestAppointments.CurrentRow.Cells["TestAppointmentID"].Value;
                TakingDrivingTestForm form = new TakingDrivingTestForm(testAppointmentId);
                form.ShowDialog();
                RefreshAppointmentsList();
            }
        }
    }
}
