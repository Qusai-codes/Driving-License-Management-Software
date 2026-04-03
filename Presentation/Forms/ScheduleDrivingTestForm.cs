using Business;
using Presentation.Controls;
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

namespace Presentation.Forms
{
    public partial class ScheduleDrivingTestForm : Form
    {
        private int _localDrivingLicenseApplicationId;
        private TestType.TestTypeId _testTypeId;
        private bool _retakeTest;
        private FormMode _mode;
        private int _testAppointmentId;

        public ScheduleDrivingTestForm(int localDrivingLicenseApplicationId, 
            TestType.TestTypeId testTypeId, FormMode mode, 
            int testAppointmentId)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
            _testTypeId = testTypeId;
            _mode = mode;
            _testAppointmentId = testAppointmentId;
        }

        private void LoadApplicationInfo()
        {
            var localDrivingLicenseApp = LocalDrivingLicenseApplication.Find(_localDrivingLicenseApplicationId);
            if (localDrivingLicenseApp == null)
            {
                MessageBox.Show("Local driving license application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int drivingLicenseApplicationId = localDrivingLicenseApp.ApplicationId;
            var drivingLicenseApp = Business.Application.FindByApplicationId(drivingLicenseApplicationId);
            if (drivingLicenseApp == null)
            {
                MessageBox.Show("Application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int personId = drivingLicenseApp.PersonId;
            int userId = drivingLicenseApp.UserId;

            string licenseClassName = string.Empty;
            DataTable classes = LicenseClass.GetAllLicenseClasses();

            if (classes != null && classes.Rows.Count > 0)
            {
                DataRow row = classes.AsEnumerable()
                    .FirstOrDefault(r => r.Field<int>("LicenseClassID") == localDrivingLicenseApp.LicenseClassId);

                if (row != null)
                {
                    licenseClassName = row.Field<string>("ClassName");
                }
            }
            _retakeTest = HasApplicantFailedTest();
            if (_retakeTest)
            {
                lblRetakeTestNotice.Text = "Person already sat for the test, " +
                    "appointment locked.";
                grpRetakeTestInfo.Enabled = true;
                lblTestTitle.Text = "Schedule Retake Test";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                lblRetakeTestNotice.Visible = false;
                grpRetakeTestInfo.Enabled = false;
                lblTestTitle.Text = "Schedule Test";
            }

            lblLocalDrivingLicenseAppId.Text = _localDrivingLicenseApplicationId.ToString();
            lblDrivingLicenseClass.Text = string.IsNullOrEmpty(licenseClassName) ? "(unknown)" : licenseClassName;
            int numberOfPassedTests = LocalDrivingLicenseApplication.GetNumberOfPassedTests(_localDrivingLicenseApplicationId);
            lblTestFees.Text = TestType.Find((int)_testTypeId).Fees.ToString();
            
            if (_mode == FormMode.Add)
            {
                dtpTestDate.Text = DateTime.Now.ToString();
            }
            else
            {
                TestAppointment testAppointment;
                testAppointment = TestAppointment.Find(_testAppointmentId);
                dtpTestDate.Value = testAppointment.AppointmentDate;

            }

            dtpTestDate.MinDate = DateTime.Now;

            lblApplicantName.Text = Person.GetFullName(personId);
            lblTrialNumber.Text = TestAppointment.GetNumberOfTestTrials(_localDrivingLicenseApplicationId,
                _testTypeId).ToString();
            lblTotalFees.Text = CalculateTotalFees().ToString();

            switch (_testTypeId)
            {
                case TestType.TestTypeId.Vision:
                    picDrivingTestType.Image = Resources.vision_test_symbol;
                    break;
                case TestType.TestTypeId.Written:
                    picDrivingTestType.Image = Resources.written_test_symbol;
                    break;
                case TestType.TestTypeId.Street:
                    picDrivingTestType.Image = Resources.street_test_symbol;
                    break;
                default:
                    break;
            }
        }

        private bool HasApplicantFailedTest()
        {
            return TestAppointment.GetTestResult(_localDrivingLicenseApplicationId, _testTypeId);
        }

        private void ScheduleDrivingTestForm_Load(object sender, EventArgs e)
        {
            LoadApplicationInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TestAppointment testAppointment; 
            if (_mode == FormMode.Add)
            {
                testAppointment = new TestAppointment();
                testAppointment.TestTypeID = (int)_testTypeId;
                testAppointment.LocalDrivingLicenseApplicationID = _localDrivingLicenseApplicationId;
                testAppointment.AppointmentDate = dtpTestDate.Value;
                testAppointment.PaidFees = CalculateTotalFees();
                testAppointment.CreatedByUserID = AppSession.CurrentUserId;
                testAppointment.IsLocked = false;
            }
            else
            {
                testAppointment = TestAppointment.Find(_testAppointmentId);
                testAppointment.AppointmentDate = dtpTestDate.Value;
            }

            if (testAppointment.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Falied to save test appointment", 
                    "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            if (_mode == FormMode.Add)
            {
                _mode = FormMode.Edit;
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private decimal CalculateTotalFees()
        {
            // UNDONE: complete the calculation logic for retaking test fees.
            return TestType.Find((int)_testTypeId).Fees;
        }
    }
}
