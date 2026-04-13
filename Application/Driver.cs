using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Driver
    {
        public EntityMode Mode { get; private set; }
        public int DriverId { get; set; }
        public int PersonId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public Driver()
        {
            DriverId = -1;
            PersonId = -1;
            CreatedByUserId = -1;
            CreatedDate = DateTime.Now;

            Mode = EntityMode.AddNew;
        }

        private Driver(int driverId, int personId, int createdByUserId, 
            DateTime createdDate)
        {
            this.DriverId = driverId;
            this.PersonId = personId;
            this.CreatedByUserId = createdByUserId;
            this.CreatedDate = createdDate;
        }

        private bool AddNew()
        {
            int driverId = DriverData.AddNewDriver(
                PersonId, CreatedByUserId, CreatedDate);
            return driverId != -1;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNew())
                    {
                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;
            }

            return false;
        }

        public static Driver FindByDriverId(int driverId)
        {
            int personId = -1, createdByUserId = -1;
            DateTime createdDate = DateTime.Now;

            if (DriverData.GetDriverInfoById(driverId, ref personId, ref createdByUserId, 
                ref createdDate))
            {
                return new Driver(driverId, personId, createdByUserId, createdDate);
            }

            return null;
        }

        public static DataTable GetAllDrivers()
        {
            return DriverData.GetAllDrivers();
        }

        public static DataTable GetAllDrivingLicensesOfPerson(int personId)
        {
            // UNDONE: complete the implementation.
            return null;
        }

        public static int GetDriverIdByPersonId(int personId)
        {
            return DriverData.GetDriverIdByPersonId(personId);
        }
    }
}
