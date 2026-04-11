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
