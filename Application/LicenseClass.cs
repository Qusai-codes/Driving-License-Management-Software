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

        public static DataTable GetAllLicenseClasses()
        {
            return LicenseClassData.GetAllLicenseClasses();
        }
    }
}
