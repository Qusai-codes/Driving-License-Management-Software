using Business.Common;
using Business.Security;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Application
    {
        public EntityMode Mode { get; private set; }

        public int ApplicationId { get; set; }
        public int PersonId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeId { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int UserId { get; set; }

        public Application()
        {
            ApplicationId = -1;
            PersonId = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeId = -1;
            ApplicationStatus = 0;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            UserId = -1;

            Mode = EntityMode.AddNew;
        }

        private Application(int applicationId, int personId, DateTime applicationDate, 
            int applicationTypeId, byte applicationStatus, DateTime lastStatusDate,
            decimal paidFees, int userId)
        {
            ApplicationId = applicationId;
            PersonId = personId;
            ApplicationDate = applicationDate;
            ApplicationTypeId = applicationTypeId;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            UserId = userId;

            Mode = EntityMode.Update;
        }

        private bool AddNewApplication()
        {
            ApplicationId = ApplicationData.AddNewApplication(PersonId, ApplicationDate, 
                ApplicationTypeId, ApplicationStatus, LastStatusDate, PaidFees, UserId);
            return ApplicationId != -1;
        }
        
        private bool UpdateApplication()
        {
            return ApplicationData.UpdateApplication(ApplicationId, ApplicationStatus, 
                LastStatusDate, PaidFees);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (CheckSameApplicationExists())
                    {
                        return false;
                    }
                    if (AddNewApplication())
                    {
                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;

                case EntityMode.Update:
                    return UpdateApplication();
            }

            return false;
        }

        /// <summary>
        /// Checks if there is an application with the user id
        /// and same application type in the database, if there is
        /// then return true.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool CheckSameApplicationExists()
        {
            throw new NotImplementedException();
        }

        public static DataTable GetAllDrivingLicenseApplications()
        {
            return ApplicationData.GetAllDrivingLicenseApplications();
        }

        public static Application FindByApplicationId(int applicationId)
        {
            int personId = -1, applicationTypeId = -1, userId = -1;
            byte applicationStatus = 0;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            decimal paidFees = 0;

            if (ApplicationData.GetApplicationInfoById(applicationId, ref personId, 
                ref applicationDate, ref applicationTypeId, ref applicationStatus, 
                ref lastStatusDate, ref paidFees, ref userId))
            {
                return new Application(applicationId, personId, applicationDate, applicationTypeId, 
                    applicationStatus, lastStatusDate, paidFees, userId);
            }
            else
            {
                return null;
            }
        }

        public static bool DeleteApplication(int applicationId)
        {
            return ApplicationData.DeleteApplication(applicationId);
        }
    }
}
