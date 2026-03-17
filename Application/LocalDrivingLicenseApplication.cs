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
    }
}
