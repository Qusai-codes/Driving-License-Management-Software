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
            NewLocalDrivingLicense,
            RenewDrivingLicense,
            ReplaceLostDrivingLicense,
            ReplaceDamagedDrivingLicense,
            ReleaseDetainedLicense,
            NewInternationalDrivingLicense,
            RetakeTest
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
            decimal fees = -1;

            switch (applicationType)
            {
                case ApplicationTypeTitle.NewLocalDrivingLicense:
                    fees = ApplicationTypeData.GetApplicationTypeFees("New Local Driving License Service");
                    break;
                case ApplicationTypeTitle.RenewDrivingLicense:
                    fees = ApplicationTypeData.GetApplicationTypeFees("Renew Driving License Service");
                    break;
                case ApplicationTypeTitle.ReplaceLostDrivingLicense:
                    fees = ApplicationTypeData.GetApplicationTypeFees("Replacement for a Lost Driving License");
                    break;
                case ApplicationTypeTitle.ReplaceDamagedDrivingLicense:
                    fees = ApplicationTypeData.GetApplicationTypeFees("Replacement for a Damaged Driving License");
                    break;
                case ApplicationTypeTitle.ReleaseDetainedLicense:
                    fees = ApplicationTypeData.GetApplicationTypeFees("Release Detained Driving Licsense");
                    break;
                case ApplicationTypeTitle.NewInternationalDrivingLicense:
                    fees = ApplicationTypeData.GetApplicationTypeFees("New International License");
                    break;
                case ApplicationTypeTitle.RetakeTest:
                    fees = ApplicationTypeData.GetApplicationTypeFees("Retake Test");
                    break;
                default:
                    break;
            }

            return fees;
        }
    }
}
