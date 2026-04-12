using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs
{
    public class DriverLicenseInfoDto
    {
        // Properties related to person information
        public string NationalNumber { get; set; }
        public string FullName { get; set; }
        public byte Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImagePath { get; set; }

        // Properties related to driving license information
        public int LicenseClassId { get; set; }
        public int LicenseId { get; set; }
        public DateTime IssueDate { get; set; }
        public byte IssueReason { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetained { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Properties related to driver
        public int DriverId { get; set; }

    }
}
