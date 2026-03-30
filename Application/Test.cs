using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Test
    {
        public EntityMode Mode { get; private set; }
        public int TestID { get; set; }
        public int TestAppointmentId { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public Test()
        {
            TestID = -1;
            TestAppointmentId = -1;
            TestResult = false;
            Notes = string.Empty;
            CreatedByUserID = -1;

            Mode = EntityMode.AddNew;
        }

        private Test(int testId, int testAppointmentId, bool testResult, 
            string notes, int createdByUserId)
        {
            this.TestID = testId;
            this.TestAppointmentId = testAppointmentId;
            this.TestResult = testResult;
            this.Notes = notes;
            this.CreatedByUserID = createdByUserId;

            Mode = EntityMode.Update;
        }

        private bool AddNewTest()
        {
            TestID =  TestData.AddNewTest(TestAppointmentId, TestResult, Notes, CreatedByUserID);
            return TestID != -1;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNewTest())
                    {
                        // Lock the related test appointment after taking the test.
                        TestAppointment appointment = TestAppointment.Find(TestAppointmentId);
                        if (appointment == null)
                        {
                            return false;
                        }

                        appointment.IsLocked = true;

                        if (!appointment.Save())
                        {
                            return false;
                        }

                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;
            }

            return false;
        }

        public static Test Find(int testId)
        {
            int testAppointmentId = -1, createdByUserId = -1;
            bool testResult = false;
            string notes = string.Empty;

            if (TestData.GetTestInfoById(testId, ref testAppointmentId, ref testResult, 
                ref notes, ref createdByUserId))
            {
                return new Test(testId, testAppointmentId, testResult, notes, createdByUserId);
            }

            return null;
        }
    }
}
