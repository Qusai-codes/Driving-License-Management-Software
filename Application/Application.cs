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
        public enum Status
        {
            New = 1,
            Canceled = 2,
            Completed = 3
        }

        public EntityMode Mode { get; private set; }

        public int ApplicationId { get; set; }
        public int PersonId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeId { get; set; }
        //public byte ApplicationStatus { get; set; }
        public Status ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int UserId { get; set; }

        public Application()
        {
            ApplicationId = -1;
            PersonId = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeId = -1;
            ApplicationStatus = Status.New;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            UserId = -1;

            Mode = EntityMode.AddNew;
        }

        private Application(int applicationId, int personId, DateTime applicationDate, 
            int applicationTypeId, Status applicationStatus, DateTime lastStatusDate,
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
                ApplicationTypeId, (byte)ApplicationStatus, LastStatusDate, PaidFees, UserId);
            return ApplicationId != -1;
        }
        
        private bool UpdateApplication()
        {
            return ApplicationData.UpdateApplication(ApplicationId, (byte)ApplicationStatus, 
                LastStatusDate);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
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

        public static DataTable GetAllDrivingLicenseApplications()
        {
            return ApplicationData.GetAllDrivingLicenseApplications();
        }

        public static Application FindByApplicationId(int applicationId)
        {
            int personId = -1, applicationTypeId = -1, userId = -1;
            byte applicationStatusByte = 0;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            decimal paidFees = 0;

            if (ApplicationData.GetApplicationInfoById(applicationId, ref personId,
                ref applicationDate, ref applicationTypeId, ref applicationStatusByte,
                ref lastStatusDate, ref paidFees, ref userId))
            {
                Status applicationStatus = Enum.IsDefined(typeof(Status), (int)applicationStatusByte)
                    ? (Status)applicationStatusByte
                    : Status.New;

                return new Application(applicationId, personId, applicationDate, applicationTypeId,
                    applicationStatus, lastStatusDate, paidFees, userId);
            }

            return null;
        }

        public static bool DeleteApplication(int applicationId)
        {
            return ApplicationData.DeleteApplication(applicationId);
        }

        public static Status GetApplicationStatus(int applicationId)
        {
            byte status = ApplicationData.GetApplicationStatus(applicationId);

            return Enum.IsDefined(typeof(Status), (int)status)
                ? (Status)status
                : Status.New;
        }

        public static int GetLatestRetakeTestApplicationId(int personId)
        {
            throw new NotImplementedException();
        }
    }
}
