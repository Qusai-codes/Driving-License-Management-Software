using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.TestType;

namespace Business
{
    public class TestAppointment
    {
        public EntityMode Mode { get; private set; }

        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }

        public TestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
            IsLocked = false;

            Mode = EntityMode.AddNew;
        }

        private TestAppointment(int testAppointmentId, int testTypeId,
            int localDrivingLicenseApplicationId, DateTime appointmentDate,
            decimal paidFees, int createdByUserId, bool isLocked)
        {
            TestAppointmentID = testAppointmentId;
            TestTypeID = testTypeId;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationId;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserId;
            IsLocked = isLocked;

            Mode = EntityMode.Update;
        }

        public static DataTable GetAllTestAppointments(int localDrivingLicenseAppId,
            TestTypeId testTypeId)
        {
            return TestAppointmentData.GetAllTestAppointments(localDrivingLicenseAppId, 
                (int)testTypeId);
        }

        private bool AddNew()
        {
            TestAppointmentID = TestAppointmentData.CreateNewTestAppointment(
                TestTypeID,
                LocalDrivingLicenseApplicationID,
                AppointmentDate,
                PaidFees,
                CreatedByUserID,
                IsLocked);

            return TestAppointmentID != -1;
        }

        private bool Update()
        {
            return TestAppointmentData.UpdateTestAppointment(TestAppointmentID, AppointmentDate, IsLocked);
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

                case EntityMode.Update:
                    return Update();
            }

            return false;
        }

        public static TestAppointment Find(int testAppointmentId)
        {
            int testTypeId = -1, localDrivingLicenseApplicationId = -1, createdByUserId = -1;
            DateTime appointmentDate = DateTime.Now;
            decimal paidFees = 0;
            bool isLocked = false;

            if (TestAppointmentData.GetTestAppointmentInfoById(testAppointmentId, ref testTypeId, 
                ref localDrivingLicenseApplicationId, ref appointmentDate, ref paidFees, 
                ref createdByUserId, ref isLocked))
            {
                return new TestAppointment(testAppointmentId, testTypeId, localDrivingLicenseApplicationId, 
                        appointmentDate, paidFees, createdByUserId, isLocked);
            }
            return null;
        }

        public static bool Delete(int testAppointmentId)
        {
            return TestAppointmentData.DeleteTestAppointment(testAppointmentId);
        }

        public static bool DoesTestAppointmentExist(int localDrivingLicenseApplicationId,
            TestTypeId testType)
        {
            return TestAppointmentData.DoesTestAppointmentExist(localDrivingLicenseApplicationId, 
                (int) testType);
        }

        public static int GetNumberOfTestTrials(int localDrivingLicenseApplicationId,
            TestTypeId testType)
        {
            return TestAppointmentData.GetNumberOfTestTrials(localDrivingLicenseApplicationId,
                (int)testType);
        }

        public static bool GetTestResult(int localDrivingLicenseApplicationId,
            TestTypeId testType)
        {
            return TestAppointmentData.GetTestResult(localDrivingLicenseApplicationId,
                (int)testType);
        }
    }
}
