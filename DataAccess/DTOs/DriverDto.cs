using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class DriverDto
    {
        public int DriverId { get; set; }
        public int PersonId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
