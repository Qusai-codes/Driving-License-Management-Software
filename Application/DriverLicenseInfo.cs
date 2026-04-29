using DataAccess.Data;
using System;

namespace Business
{
    public class DriverLicenseInfo
    {
        public string NationalNumber { get; private set; }
        public string FullName { get; private set; }
        public byte Gender { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string ImagePath { get; private set; }

        public int LicenseClassId { get; private set; }
        public int LicenseId { get; private set; }
        public DateTime IssueDate { get; private set; }
        public byte IssueReason { get; private set; }
        public string Notes { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDetained { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public int DriverId { get; private set; }

        private DriverLicenseInfo(int driverId, string nationalNumber, string fullName, byte gender,
            DateTime dateOfBirth, string imagePath, int licenseClassId, int licenseId,
            DateTime issueDate, byte issueReason, string notes, bool isActive, bool isDetained,
            DateTime expirationDate)
        {
            DriverId = driverId;
            NationalNumber = nationalNumber;
            FullName = fullName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            ImagePath = imagePath;

            LicenseClassId = licenseClassId;
            LicenseId = licenseId;
            IssueDate = issueDate;
            IssueReason = issueReason;
            Notes = notes;
            IsActive = isActive;
            IsDetained = isDetained;
            ExpirationDate = expirationDate;
        }

        public static DriverLicenseInfo GetByLicenseId(int licenseId)
        {
            int driverId = -1;
            string nationalNumber = "", fullName = "", imagePath = null, notes = null;
            byte gender = 0, issueReason = 0;
            DateTime dateOfBirth = DateTime.MinValue, issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            int licenseClassId = -1, licenseIdOut = -1;
            bool isActive = false, isDetained = false;

            if (DriverLicenseInfoData.GetDriverLicenseInfoByLicenseId(licenseId, ref driverId,
                ref nationalNumber, ref fullName, ref gender, ref dateOfBirth, ref imagePath,
                ref licenseClassId, ref licenseIdOut, ref issueDate, ref issueReason, ref notes,
                ref isActive, ref isDetained, ref expirationDate))
            {
                return new DriverLicenseInfo(driverId, nationalNumber, fullName, gender, dateOfBirth,
                    imagePath, licenseClassId, licenseIdOut, issueDate, issueReason, notes, isActive,
                    isDetained, expirationDate);
            }

            return null;
        }

        public static DriverLicenseInfo GetByDriverId(int driverId)
        {
            int driverIdOut = -1;
            string nationalNumber = "", fullName = "", imagePath = null, notes = null;
            byte gender = 0, issueReason = 0;
            DateTime dateOfBirth = DateTime.MinValue, issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            int licenseClassId = -1, licenseId = -1;
            bool isActive = false, isDetained = false;

            if (DriverLicenseInfoData.GetDriverLicenseInfoByDriverId(driverId, ref driverIdOut,
                ref nationalNumber, ref fullName, ref gender, ref dateOfBirth, ref imagePath,
                ref licenseClassId, ref licenseId, ref issueDate, ref issueReason, ref notes,
                ref isActive, ref isDetained, ref expirationDate))
            {
                return new DriverLicenseInfo(driverIdOut, nationalNumber, fullName, gender, dateOfBirth,
                    imagePath, licenseClassId, licenseId, issueDate, issueReason, notes, isActive,
                    isDetained, expirationDate);
            }

            return null;
        }
    }
}
