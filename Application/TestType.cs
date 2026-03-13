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
    public class TestType
    {
        public EntityMode Mode { get; private set; }

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }

        private TestType(int id, string title, string description, decimal fees)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Fees = fees;

            Mode = EntityMode.Update;
        }

        private bool AddNewTestType()
        {
            return false;
        }

        private bool UpdateTestType()
        {
            return TestTypeData.UpdateTestType(this.Id, this.Title, 
                this.Description, this.Fees);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNewTestType())
                    {
                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;

                case EntityMode.Update:
                    return UpdateTestType();
            }

            return false;
        }

        public static DataTable GetAllTestTypes()
        {
            return TestTypeData.GetAllTestTypes();
        }

        public static TestType Find(int id)
        {
            string title = "", description = "";
            decimal fees = 0;

            if (TestTypeData.GetTestTypeInfoById(id, ref title,
                ref description, ref fees))
            {
                return new TestType(id, title, description, fees);
            }
            else
            {
                return null;
            }
        }
    }
}
