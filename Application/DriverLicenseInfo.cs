using Contracts.DTOs;
using DataAccess.Data;

namespace Business
{
    public static class DriverLicenseInfo
    {
        public static DriverLicenseInfoDto GetByLicenseId(int licenseId)
        {
            return DriverLicenseInfoData.GetDriverLicenseInfoByLicenseId(licenseId);
        }

        // Optional compatibility method (can be removed later if not needed).
        public static DriverLicenseInfoDto GetByDriverId(int driverId)
        {
            return DriverLicenseInfoData.GetDriverLicenseInfoByDriverId(driverId);
        }
    }
}
