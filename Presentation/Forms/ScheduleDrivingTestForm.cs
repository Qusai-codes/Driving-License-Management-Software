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
        private int _retakeTestApplicationId;
        private bool _isEditLockedAppointment;

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

            TestAppointment currentAppointment = null;
            if (_mode == FormMode.Edit)
            {
                currentAppointment = TestAppointment.Find(_testAppointmentId);
                if (currentAppointment == null)
                {
                    MessageBox.Show("Test appointment not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (_mode == FormMode.Edit && _retakeTest)
            {
                _retakeTestApplicationId = Business.Application.GetLatestRetakeTestApplicationId(personId);

                if (_retakeTestApplicationId == -1)
                {
                    MessageBox.Show("Retake test application not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lblRetakeTestAppId.Text = _retakeTestApplicationId.ToString();
            }

            _isEditLockedAppointment =
                _mode == FormMode.Edit &&
                currentAppointment != null &&
                currentAppointment.IsLocked;

            if (_isEditLockedAppointment)
            {
                lblRetakeTestNotice.Text = "Person already sat for the test, appointment locked.";
                lblRetakeTestNotice.Visible = true;
                grpRetakeTestInfo.Enabled = true;
                lblTestTitle.Text = "Schedule Retake Test";

                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
            }
            else if (_retakeTest)
            {
                lblRetakeTestNotice.Visible = false;
                grpRetakeTestInfo.Enabled = true;
                lblTestTitle.Text = "Schedule Retake Test";

                dtpTestDate.Enabled = true;
                btnSave.Enabled = true;
                lblRetakeTestApplicationFee.Text = ApplicationType.GetApplicationTypeFees(
                    ApplicationType.ApplicationTypeTitle.RetakeTest).ToString();
            }
            else
            {
                lblRetakeTestNotice.Visible = false;
                grpRetakeTestInfo.Enabled = false;
                lblTestTitle.Text = "Schedule Test";

                dtpTestDate.Enabled = true;
                btnSave.Enabled = true;
            }

            if (_mode == FormMode.Add)
            {
                dtpTestDate.Value = DateTime.Now;
            }
            else
            {
                dtpTestDate.Value = currentAppointment.AppointmentDate;
            }

            dtpTestDate.MinDate = DateTime.Now;

            lblLocalDrivingLicenseAppId.Text = _localDrivingLicenseApplicationId.ToString();
            lblDrivingLicenseClass.Text = string.IsNullOrEmpty(licenseClassName) ? "(unknown)" : licenseClassName;
            lblApplicantName.Text = Person.GetFullName(personId);
            lblTrialNumber.Text = TestAppointment.GetNumberOfTestTrials(
                _localDrivingLicenseApplicationId, _testTypeId).ToString();
            lblTestFees.Text = TestType.Find((int)_testTypeId).Fees.ToString();
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
            }
        }

        private bool HasApplicantFailedTest()
        {
            int trials = TestAppointment.GetNumberOfTestTrials(_localDrivingLicenseApplicationId, _testTypeId);
            if (trials == 0)
                return false;

            return !TestAppointment.GetTestResult(_localDrivingLicenseApplicationId, _testTypeId);
        }

        private void ScheduleDrivingTestForm_Load(object sender, EventArgs e)
        {
            LoadApplicationInfo();
        }

        private bool TryCreateRetakeApplicationIfNeeded()
        {
            if (!HasApplicantFailedTest())
                return true;

            int currentApplicationId = LocalDrivingLicenseApplication.GetApplicationId(_localDrivingLicenseApplicationId);

            Business.Application currentApplication = Business.Application.FindByApplicationId(currentApplicationId);

            Business.Application retakeTestApplication = new Business.Application
            {
                PersonId = currentApplication.PersonId,
                ApplicationDate = DateTime.Now,
                ApplicationTypeId = (int)ApplicationType.ApplicationTypeTitle.RetakeTest,
                ApplicationStatus = Business.Application.Status.New,
                LastStatusDate = DateTime.Now,
                PaidFees = ApplicationType.GetApplicationTypeFees(ApplicationType.ApplicationTypeTitle.RetakeTest),
                UserId = AppSession.CurrentUserId
            };

            if (!retakeTestApplication.Save())
            {
                MessageBox.Show("Unable to create retake test application.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            _retakeTestApplicationId = retakeTestApplication.ApplicationId;

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_mode == FormMode.Add)
            {
                if (!TryCreateRetakeApplicationIfNeeded())
                    return;
            }

            TestAppointment testAppointment;
            testAppointment = new TestAppointment();
            if (_mode == FormMode.Add)
            {
                
                testAppointment.TestTypeID = (int)_testTypeId;
                testAppointment.LocalDrivingLicenseApplicationID = _localDrivingLicenseApplicationId;
                testAppointment.AppointmentDate = dtpTestDate.Value;
                testAppointment.PaidFees = CalculateTotalFees();
                testAppointment.CreatedByUserID = AppSession.CurrentUserId;
                testAppointment.IsLocked = false;
                testAppointment.RetakeTestApplicationID = _retakeTest ? _retakeTestApplicationId
                                                       : -1;

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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private decimal CalculateTotalFees()
        {
            decimal testFees = TestType.Find((int)_testTypeId).Fees;

            if (_retakeTest && !_isEditLockedAppointment)
            {
                decimal retakeApplicationFees = ApplicationType.GetApplicationTypeFees(
                    ApplicationType.ApplicationTypeTitle.RetakeTest);

                return testFees + retakeApplicationFees;
            }

            return testFees;
        }
    }
}
