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
    public partial class TakingDrivingTestForm : Form
    {
        private int _testAppointmentId;

        public TakingDrivingTestForm(int testAppointmentId)
        {
            InitializeComponent();

            _testAppointmentId = testAppointmentId;
        }

        private void TakingDrivingTestForm_Load(object sender, EventArgs e)
        {
            LoadTestAppointmentData();
        }

        private void LoadTestAppointmentData()
        {
            TestAppointment appointment = TestAppointment.Find(_testAppointmentId);
            if (appointment == null)
            {
                MessageBox.Show("Test appointment not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LocalDrivingLicenseApplication localDrivingLicenseApp =
                LocalDrivingLicenseApplication.Find(appointment.LocalDrivingLicenseApplicationID);

            if (localDrivingLicenseApp == null)
            {
                MessageBox.Show("Local driving license application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Business.Application drivingLicenseApp =
                Business.Application.FindByApplicationId(localDrivingLicenseApp.ApplicationId);

            if (drivingLicenseApp == null)
            {
                MessageBox.Show("Application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int personId = drivingLicenseApp.PersonId;

            TestType.TestTypeId testTypeId = TestType.TestTypeId.Vision;
            if (Enum.IsDefined(typeof(TestType.TestTypeId), appointment.TestTypeID))
            {
                testTypeId = (TestType.TestTypeId)appointment.TestTypeID;
            }

            string licenseClassName = "(unknown)";
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

            TestType testType = TestType.Find((int)testTypeId);

            lblLocalDrivingLicenseAppId.Text = localDrivingLicenseApp.LocalDrivingLicenseApplicationId.ToString();
            lblDrivingLicenseClass.Text = licenseClassName;
            lblApplicantName.Text = Person.GetFullName(personId);
            lblTrialNumber.Text = TestAppointment.GetNumberOfTestTrials(
                localDrivingLicenseApp.LocalDrivingLicenseApplicationId, testTypeId).ToString();
            lblTestFees.Text = (testType != null ? testType.Fees : 0m).ToString();
            lblTestId.Text = "Not Taken Yet";

            lblTestDate.Text = appointment.AppointmentDate.ToString("d");

            switch (testTypeId)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to save? After that you cannot" 
                + " change the Pass/Fail results after you save.", "Confirm", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Test newDrivingTest = new Test();
                newDrivingTest.TestAppointmentId = _testAppointmentId;
                newDrivingTest.TestResult = rdoPassTest.Checked ? true : false;
                newDrivingTest.Notes = txtNotes.Text;
                newDrivingTest.CreatedByUserID = AppSession.CurrentUserId;

                if (newDrivingTest.Save())
                {
                    MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to Save Data.", "Fail", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }

                this.Close();
            }

        }
    }
}
