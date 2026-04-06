using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ApplicationType
    {
        public enum ApplicationTypeTitle
        {
            NewLocalDrivingLicense = 1,
            RenewDrivingLicense = 2,
            ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4,
            ReleaseDetainedLicense = 5,
            NewInternationalDrivingLicense = 6,
            RetakeTest = 8
        }

        public EntityMode Mode { get; private set; }

        public int Id { get; private set; }
        public string Title { get; set; }
        public decimal Fees { get; set; }

        private ApplicationType(int id, string title, decimal fees)
        {
            this.Id = id;
            this.Title = title;
            this.Fees = fees;

            Mode = EntityMode.Update;
        }

        private bool AddNewApplicationType()
        {
            return false;
        }

        private bool UpdateApplicationType()
        {
            return ApplicationTypeData.UpdateApplicationType(this.Id, 
                this.Title, this.Fees);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNewApplicationType())
                    {
                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;

                case EntityMode.Update:
                    return UpdateApplicationType();
            }

            return false;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return ApplicationTypeData.GetAllApplicationTypes();
        }

        public static ApplicationType Find(int id)
        {
            string title = "";
            decimal fees = 0;

            if (ApplicationTypeData.GetApplicationTypeInfoById(id, ref title, 
                ref fees))
            {
                return new ApplicationType(id, title, fees);
            }
            else
            {
                return null;
            }
        }

        public static decimal GetApplicationTypeFees(ApplicationTypeTitle applicationType)
        {
            int applicationTypeId;
            if (!TryGetApplicationTypeId(applicationType, out applicationTypeId))
            {
                return -1m;
            }

            return ApplicationTypeData.GetApplicationTypeFees(applicationTypeId);
        }

        private static bool TryGetApplicationTypeId(ApplicationTypeTitle applicationType, out int id)
        {
            switch (applicationType)
            {
                case ApplicationTypeTitle.NewLocalDrivingLicense:
                    id = 1;
                    return true;
                case ApplicationTypeTitle.RenewDrivingLicense:
                    id = 2;
                    return true;
                case ApplicationTypeTitle.ReplaceLostDrivingLicense:
                    id = 3;
                    return true;
                case ApplicationTypeTitle.ReplaceDamagedDrivingLicense:
                    id = 4;
                    return true;
                case ApplicationTypeTitle.ReleaseDetainedLicense:
                    id = 5;
                    return true;
                case ApplicationTypeTitle.NewInternationalDrivingLicense:
                    id = 6;
                    return true;
                case ApplicationTypeTitle.RetakeTest:
                    id = 8;
                    return true;
                default:
                    id = -1;
                    return false;
            }
        }

        public static string GetApplicationTypeTitle(int applicationTypeId)
        {
            return ApplicationTypeData.GetApplicationTypeTitle(applicationTypeId);
        }
    }
}
