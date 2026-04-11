using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class License
    {
        public enum IssueReasonType : byte
        {
            FirstTime = 1,
            Renew = 2,
            ReplacementForDamaged = 3,
            ReplacementForLost = 4
        }

        public EntityMode Mode { get; set; }
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public byte IssueReason { get; set; }
        public int CreatedByUserID { get; set; }

        public License()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            Notes = string.Empty;
            PaidFees = 0;
            IsActive = false;
            IssueReason = 0;
            CreatedByUserID = -1;

            Mode = EntityMode.AddNew;
        }

        private License(int licenseId, int applicationId, int driverId, int licenseClass,
            DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, 
            bool isActive, byte issueReason, int createdByUserId)
        {
            LicenseID = licenseId;
            ApplicationID = applicationId;
            DriverID = driverId;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserId;

            Mode = EntityMode.Update;
        }

        private bool AddNew()
        {
            LicenseID = LicenseData.AddNewLicense(
                ApplicationID,
                DriverID,
                LicenseClass,
                IssueDate,
                ExpirationDate,
                Notes,
                PaidFees,
                IsActive,
                IssueReason,
                CreatedByUserID);

            return LicenseID != -1;
        }

        private bool Update()
        {
            return LicenseData.UpdateLicense(LicenseID, IsActive);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNew())
                    {
                        Mode = EntityMode.Update;
                        UpdateApplicationStatusToComplete();
                        return true;
                    }
                    return false;

                case EntityMode.Update:
                    return Update();
            }

            return false;
        }

        private void UpdateApplicationStatusToComplete()
        {
            Application application = Application.FindByApplicationId(ApplicationID);
            if (application != null)
            {
                application.ApplicationStatus = Application.Status.Completed;
                application.LastStatusDate = DateTime.Now;
                application.Save();
            }
        }

        public static License Find(int licenseId)
        {
            int applicationId = -1, driverId = -1, licenseClassId = -1, createdByUserId = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            string notes = string.Empty;
            decimal paidFees = 0;
            bool isActive = false;
            byte issueReason = 0;

            if (LicenseData.GetLicenseInfoById(licenseId, ref applicationId, ref driverId,
                ref licenseClassId, ref issueDate, ref expirationDate, ref notes,
                ref paidFees, ref isActive, ref issueReason, ref createdByUserId))
            {
                return new License(licenseId, applicationId, driverId, licenseClassId,
                    issueDate, expirationDate, notes, paidFees, isActive, issueReason, createdByUserId);
            }

            return null;
        }

        public static bool DoesLicenseExist(int applicationId)
        {
            return LicenseData.GetLicenseId(applicationId) != -1;
        }

        public static bool DoesActiveLicenseExistForDriverAndClass(int driverId, int licenseClassId)
        {
            return LicenseData.DoesActiveLicenseExistForDriverAndClass(driverId, licenseClassId);
        }


    }
}
