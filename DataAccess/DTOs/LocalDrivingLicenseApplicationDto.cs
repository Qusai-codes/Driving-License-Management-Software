using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class LocalDrivingLicenseApplicationDto
    {
        public int LocalDrivingLicenseApplicationId { get; set; }
        public int ApplicationId { get; set; }
        public int LicenseClassId { get; set; }
    }
}
