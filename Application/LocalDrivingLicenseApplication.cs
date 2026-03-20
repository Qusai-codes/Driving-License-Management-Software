using Business.Common;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class LocalDrivingLicenseApplication
    {
        public EntityMode Mode { get; private set; }

        public int LocalDrivingLicenseApplicationId { get; set; }
        public int ApplicationId { get; set; }
        public int LicenseClassId { get; set; }

        public LocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationId = -1;
            ApplicationId = -1;
            LicenseClassId = -1;

            Mode = EntityMode.AddNew;
        }

        private LocalDrivingLicenseApplication(int localDrivingLicenseApplicationId, 
            int applicationId, int drivingLicenseClassId)
        {
            LocalDrivingLicenseApplicationId = localDrivingLicenseApplicationId;
            ApplicationId = applicationId;
            LicenseClassId = drivingLicenseClassId;

            Mode = EntityMode.Update;
        }

        private bool AddNewApplication()
        {
            // Check if there is application with the same license class the person
            LocalDrivingLicenseApplicationId = LocalDrivingLicenseApplicationData.AddNewApplication(
                ApplicationId, LicenseClassId);
            return LocalDrivingLicenseApplicationId != -1;
        }

        public bool Save()
        {
            if (Mode != EntityMode.AddNew)
            {
                // No update operation for this table by design
                return false;
            }

            if (AddNewApplication())
            {
                Mode = EntityMode.Update;
                return true;
            }

            return false;
        }

        public static bool DeleteApplication(int applicationId)
        {
            return LocalDrivingLicenseApplicationData.DeleteApplication(applicationId);
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return LocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public static LocalDrivingLicenseApplication Find(int localDrivingLicenseApplicationId)
        {
            int applicationId = -1;
            int licenseClassId = -1;

            if (LocalDrivingLicenseApplicationData.GetApplicationInfoById(
                localDrivingLicenseApplicationId, ref applicationId, ref licenseClassId))
            {
                return new LocalDrivingLicenseApplication(
                    localDrivingLicenseApplicationId, applicationId, licenseClassId);
            }

            return null;
        }

        public static int GetApplicationId(int localApplicationId)
        {
            return LocalDrivingLicenseApplicationData.GetApplicationId(localApplicationId);
        }

        public static bool TryCreateNew(int personId, int licenseClassId, int userId,
            out int localDrivingLicenseApplicationId, out int blockingApplicationId, out string errorMessage)
        {
            localDrivingLicenseApplicationId = -1;
            blockingApplicationId = -1;
            errorMessage = string.Empty;

            // Business rule: block if same person + class has status New or Completed
            int newStatusAppId = LocalDrivingLicenseApplicationData.GetApplicationId(
                personId, licenseClassId, (byte)Application.Status.New);

            int completedStatusAppId = LocalDrivingLicenseApplicationData.GetApplicationId(
                personId, licenseClassId, (byte)Application.Status.Completed);

            if (newStatusAppId != -1 || completedStatusAppId != -1)
            {
                blockingApplicationId = newStatusAppId != -1 ? newStatusAppId : completedStatusAppId;
                errorMessage = string.Format(
                    "Choose another License Class, the selected person already has an active application for the selected class with id = {0}",
                    blockingApplicationId);
                return false;
            }

            Application baseApplication = new Application
            {
                PersonId = personId,
                ApplicationDate = DateTime.Now,
                ApplicationTypeId = (int)ApplicationType.ApplicationTypeTitle.NewLocalDrivingLicense + 1,
                ApplicationStatus = Application.Status.New,
                LastStatusDate = DateTime.Now,
                PaidFees = ApplicationType.GetApplicationTypeFees(ApplicationType.ApplicationTypeTitle.NewLocalDrivingLicense),
                UserId = userId
            };

            if (!baseApplication.Save())
            {
                errorMessage = "Unable to save new application.";
                return false;
            }

            LocalDrivingLicenseApplication localApp = new LocalDrivingLicenseApplication
            {
                ApplicationId = baseApplication.ApplicationId,
                LicenseClassId = licenseClassId
            };

            if (!localApp.Save())
            {
                Application.DeleteApplication(baseApplication.ApplicationId); // rollback parent
                errorMessage = "Unable to save local driving license application.";
                return false;
            }

            localDrivingLicenseApplicationId = localApp.LocalDrivingLicenseApplicationId;
            return true;
        }

        public static bool CancelApplication(int localApplicationId)
        {
            int applicationId = LocalDrivingLicenseApplicationData.GetApplicationId(localApplicationId);
            if (applicationId == -1)
            {
                return false;
            }

            Application application = Application.FindByApplicationId(applicationId);
            if (application == null)
            {
                return false;
            }

            if (application.ApplicationStatus == Application.Status.Canceled ||
                application.ApplicationStatus == Application.Status.Completed)
            {
                return false;
            }

            application.ApplicationStatus = Application.Status.Canceled;
            application.LastStatusDate = DateTime.Now;

            return application.Save();
        }

        public static int GetNumberOfPassedTests(int localDrivingLicenseApplicationId)
        {
            DataTable tests = LocalDrivingLicenseApplicationData.GetTestsTaken(localDrivingLicenseApplicationId);

            if (tests == null || tests.Rows.Count == 0 || !tests.Columns.Contains("TestResult"))
                return 0;

            return tests.AsEnumerable()
                        .Count(r => r.Field<bool>("TestResult"));
        }
    }
}
