using Business.Common;
using DataAccess.Data;
using System;
using System.Data;

namespace Business
{
    public class InternationalLicense
    {
        public const int InternationalLicenseValidityYears = 1;

        public EntityMode Mode { get; private set; }

        public int InternationalLicenseId { get; set; }
        public int ApplicationId { get; set; }
        public int DriverId { get; set; }
        public int IssuedUsingLocalLicenseId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserId { get; set; }

        public InternationalLicense()
        {
            InternationalLicenseId = -1;
            ApplicationId = -1;
            DriverId = -1;
            IssuedUsingLocalLicenseId = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(InternationalLicenseValidityYears);
            IsActive = true;
            CreatedByUserId = -1;

            Mode = EntityMode.AddNew;
        }

        private InternationalLicense(int internationalLicenseId, int applicationId, int driverId,
            int issuedUsingLocalLicenseId, DateTime issueDate, DateTime expirationDate,
            bool isActive, int createdByUserId)
        {
            InternationalLicenseId = internationalLicenseId;
            ApplicationId = applicationId;
            DriverId = driverId;
            IssuedUsingLocalLicenseId = issuedUsingLocalLicenseId;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUserId = createdByUserId;

            Mode = EntityMode.Update;
        }

        private bool AddNew()
        {
            InternationalLicenseId = InternationalLicenseData.AddNewInternationalDrivingLicense(
                ApplicationId,
                DriverId,
                IssuedUsingLocalLicenseId,
                IssueDate,
                ExpirationDate,
                IsActive,
                CreatedByUserId);

            return InternationalLicenseId != -1;
        }

        private bool Update()
        {
            return InternationalLicenseData.UpdateInternationalDrivingLicense(
                InternationalLicenseId,
                IsActive);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNew())
                    {
                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;

                case EntityMode.Update:
                    return Update();
            }

            return false;
        }

        public static InternationalLicense Find(int internationalLicenseId)
        {
            int applicationId = -1, driverId = -1, issuedUsingLocalLicenseId = -1, createdByUserId = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            bool isActive = false;

            if (InternationalLicenseData.GetInternationalDrivingLicenseInfoById(
                internationalLicenseId, ref applicationId, ref driverId, ref issuedUsingLocalLicenseId,
                ref issueDate, ref expirationDate, ref isActive, ref createdByUserId))
            {
                return new InternationalLicense(internationalLicenseId, applicationId, driverId,
                    issuedUsingLocalLicenseId, issueDate, expirationDate, isActive, createdByUserId);
            }

            return null;
        }

        public static DataTable GetAllInternationalDrivingLicenses()
        {
            return InternationalLicenseData.GetAllInternationalDrivingLicenses();
        }

        public static bool DoesActiveInternationalLicenseExistForDriver(int driverId)
        {
            return InternationalLicenseData.DoesActiveInternationalLicenseExistForDriver(driverId);
        }
    }
}