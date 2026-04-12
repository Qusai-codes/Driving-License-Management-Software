using Contracts.DTOs;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class DriverLicenseInfo
    {
        public static DriverLicenseInfoDto GetByDriverId(int driverId)
        {
            return DriverLicenseInfoData.GetDriverLicenseInfoByDriverId(driverId);
        }
    }
}
