using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class InternationalLicenseDto
    {
        public int InternationalLicenseId { get; set; }
        public int ApplicationId { get; set; }
        public int DriverId { get; set; }
        public int IssuedUsingLocalLicenseId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
