using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Controls
{
    public partial class DrivingLicenseApplicationInformationControl : UserControl
    {
        private int _personId;
        private int _userId;
        private int _applicationId;
        private int _localDrivingLicenseApplicationId;

        public DrivingLicenseApplicationInformationControl()
        {
            InitializeComponent();
        }

        public int LocalDrivingLicenseApplicationId
        {
            // Call the setter in the constructor of the parent Form or Load Form event.
            set 
            { 
                _localDrivingLicenseApplicationId = value;
                LoadApplicationInfo();
            }
        }

        public LinkLabel ShowLicenseInformationLinkLabel
        {
            get { return llbShowLicenseInfo; }
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

            _applicationId = localDrivingLicenseApp.ApplicationId;
            var drivingLicenseApp = Business.Application.FindByApplicationId(_applicationId);
            if (drivingLicenseApp == null)
            {
                MessageBox.Show("Application not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _personId = drivingLicenseApp.PersonId;
            _userId = drivingLicenseApp.UserId;

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

            // Filling the controls
            lblDrivingLicenseApplicationId.Text = _localDrivingLicenseApplicationId.ToString();
            lblLicenseClass.Text = string.IsNullOrEmpty(licenseClassName) ? "(unknown)" : licenseClassName;
            int numberOfTests = TestType.GetTestTypeCount();
            int numberOfPassedTests = LocalDrivingLicenseApplication.GetNumberOfPassedTests(_localDrivingLicenseApplicationId);
            lblPassedTests.Text = string.Format("{0}/{1}", numberOfPassedTests, numberOfTests);
            lblApplicationId.Text = _applicationId.ToString();
            lblApplicationStatus.Text = drivingLicenseApp.ApplicationStatus.ToString();
            lblApplicationFees.Text = drivingLicenseApp.PaidFees.ToString();
            lblApplicationType.Text = ApplicationType.GetApplicationTypeTitle(drivingLicenseApp.ApplicationTypeId);
            lblApplicantName.Text = Person.GetFullName(_personId);
            lblApplicationDate.Text = drivingLicenseApp.ApplicationDate.ToString("d");
            lblApplicationLastStatusDate.Text = drivingLicenseApp.LastStatusDate.ToString("d");
            lblUserThatCreatedApplication.Text = User.Find(_userId).UserName;

        }

        private void llbViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PersonDetailsForm form = new PersonDetailsForm(_personId);
            form.ShowDialog();
        }

        private void llbShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: Complete the implementation of this method.
        }
    }
}
