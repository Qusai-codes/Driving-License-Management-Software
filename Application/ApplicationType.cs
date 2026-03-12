using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ApplicationType
    {
        public EntityMode Mode { get; private set; }

        public int Id { get; private set; }
        public string Title { get; set; }
        public decimal Fees { get; set; }

        private ApplicationType(int id, string title, decimal fees)
        {
            this.Id = id;
            this.Title = title;
            this.Fees = fees;

            Mode = EntityMode.Update;
        }

        private bool AddNewApplicationType()
        {
            return false;
        }

        private bool UpdateApplicationType()
        {
            return ApplicationTypeData.UpdateApplicationType(this.Id, 
                this.Title, this.Fees);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNewApplicationType())
                    {
                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;

                case EntityMode.Update:
                    return UpdateApplicationType();
            }

            return false;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return ApplicationTypeData.GetAllApplicationTypes();
        }

        public static ApplicationType Find(int id)
        {
            string title = "";
            decimal fees = 0;

            if (ApplicationTypeData.GetApplicationTypeInfoById(id, ref title, 
                ref fees))
            {
                return new ApplicationType(id, title, fees);
            }
            else
            {
                return null;
            }
        }
    }
}
