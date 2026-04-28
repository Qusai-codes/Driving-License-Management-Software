using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class DetainedLicense
    {
        public EntityMode Mode { get; private set; }
        public int DetainId { get; set; }
        public int LicenseId { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }

        public DetainedLicense()
        {
            DetainId = -1;
            LicenseId = -1;
            DetainDate = DateTime.MinValue;
            FineFees = 0;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MinValue;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;

            Mode = EntityMode.AddNew;
        }

        private DetainedLicense(int detainId, int licenseId, DateTime detainDate, decimal fineFees,
            int createdByUserId, bool isReleased, DateTime releaseDate, int releasedByUserId,
            int releaseApplicationId)
        {
            DetainId = detainId;
            LicenseId = licenseId;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserId;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserId;
            ReleaseApplicationID = releaseApplicationId;

            Mode = EntityMode.Update;
        }

        private bool AddNew()
        {
            DetainId = DetainedLicenseData.AddNewDetainedDriverLicense(
                LicenseId,
                DetainDate,
                FineFees,
                CreatedByUserID,
                IsReleased,
                ReleaseDate,
                ReleasedByUserID,
                ReleaseApplicationID);

            return DetainId != -1;
        }

        private bool Update()
        {
            return DetainedLicenseData.UpdateDetainedLicense(
                DetainId,
                IsReleased,
                ReleaseDate,
                ReleasedByUserID,
                ReleaseApplicationID);
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

        public static DetainedLicense FindByLicenseId(int licenseId)
        {
            int detainId = -1;
            DateTime detainDate = DateTime.MinValue;
            decimal fineFees = 0;
            int createdByUserId = -1;
            bool isReleased = false;
            DateTime releaseDate = DateTime.MinValue;
            int releasedByUserId = -1;
            int releaseApplicationId = -1;

            if (DetainedLicenseData.GetLicenseDetainInfoByLicenseId(
                licenseId, ref detainId, ref detainDate, ref fineFees, ref createdByUserId,
                ref isReleased, ref releaseDate, ref releasedByUserId, ref releaseApplicationId))
            {
                return new DetainedLicense(detainId, licenseId, detainDate, fineFees, createdByUserId,
                    isReleased, releaseDate, releasedByUserId, releaseApplicationId);
            }

            return null;
        }

        public static bool IsLicenseDetained(int licenseId)
        {
            return DetainedLicenseData.IsLicenseDetained(licenseId);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return DetainedLicenseData.GetAllDetainedLicenses();
        }
    }
}
