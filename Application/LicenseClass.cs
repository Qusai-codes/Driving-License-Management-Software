using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class LicenseClass
    {
        public enum LicenseClassId
        {
            SmallMotorcycle = 1,
            HeavyMotorcycleLicense = 2,
            OrdinaryDrivingLicense = 3,
            Commercial = 4,
            Agricultural = 5,
            SmallAndMediumBus = 6,
            TruckAndHeavyVehicle = 7
        }

        public static byte GetMinimumAllowedAge(int licenseClassId)
        {
            return LicenseClassData.GetMinimumAllowedAge(licenseClassId);
        }

        public static byte GetDefaultValidityLength(int licenseClassId)
        {
            return LicenseClassData.GetDefaultValidityLength(licenseClassId);
        }

        public static DataTable GetAllLicenseClasses()
        {
            return LicenseClassData.GetAllLicenseClasses();
        }

        public static string GetLicenseClassName(int licenseClassId)
        {
            return LicenseClassData.GetLicenseClassName(licenseClassId);
        }
    }
}
