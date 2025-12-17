using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class TestAppointmentDto
    {
        public int TestAppointmentId { get; set; }
        public int LocalDrivingLicenseApplicationId { get; set; }
        public int TestTypeId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserId { get; set; }
        public bool IsLocked { get; set; }
    }
}
