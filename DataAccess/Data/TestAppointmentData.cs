using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class TestAppointmentData
    {
        public static bool GetTestAppointmentInfoById(int testAppointmentId, 
            ref int testTypeId, ref int localDrivingLicenseAppId, ref DateTime appointmentDate, 
            ref decimal paidFees, ref int createdByUserId, ref bool isLocked)
        {
            bool isFound = false;

            const string query = @"
            SELECT TestTypeID, LocalDrivingLicenseApplicationID, 
                AppointmentDate, PaidFees, CreatedByUserID, IsLocked 
            FROM TestAppointments 
            WHERE TestAppointmentID = @TestAppointmentID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = testAppointmentId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        testTypeId = (int)reader["TestTypeID"];
                        localDrivingLicenseAppId = (int)reader["LocalDrivingLicenseApplicationID"];
                        appointmentDate = (DateTime)reader["AppointmentDate"];
                        paidFees = (decimal)reader["PaidFees"];
                        createdByUserId = (int)reader["CreatedByUserID"];
                        isLocked = (bool)reader["IsLocked"];
                    }
                    else
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetAllTestAppointments(int localDrivingLicenseAppId, 
            int testTypeId)
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked 
            FROM TestAppointments 
            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
	            AND TestTypeID = @TestTypeID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = localDrivingLicenseAppId;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testTypeId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static int CreateNewTestAppointment(int testTypeId, int localDrivingLicenseAppId,
            DateTime appointmentDate, decimal paidFees, int createdByUserId, bool isLocked)
        {
            int testAppointmentId = -1;

            const string query = @"
            INSERT INTO TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, 
            AppointmentDate, PaidFees, CreatedByUserID, IsLocked) 
            VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate,
            @PaidFees, @CreatedByUserID, @IsLocked);
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testTypeId;
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = localDrivingLicenseAppId;
                command.Parameters.Add("@AppointmentDate", SqlDbType.SmallDateTime).Value = appointmentDate;
                command.Parameters.Add("@PaidFees", SqlDbType.SmallMoney).Value = paidFees;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserId; 
                command.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = isLocked;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    testAppointmentId = insertedId;
                }
            }

            return testAppointmentId;
        }

        public static bool UpdateTestAppointment(int testAppointmentId, DateTime testAppointmentDate,
            bool isLocked)
        {
            int rowsAffected = 0;

            const string query = @"
            UPDATE TestAppointments
            SET IsLocked = @IsLocked, AppointmentDate = @AppointmentDate 
            WHERE TestAppointmentID = @TestAppointmentID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = testAppointmentId;
                command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = testAppointmentDate;
                command.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = isLocked;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static bool DeleteTestAppointment(int testAppointmentId)
        {
            int rowsAffected = 0;

            const string query = @"
            DELETE FROM TestAppointments 
            WHERE TestAppointmentID = @TestAppointmentID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = testAppointmentId;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static bool DoesTestAppointmentExist(int localDrivingLicenseApplicationId, int testTypeId)
        {
            bool isFound = false;

            const string query = @"
            SELECT TestTypeID
            FROM TestAppointments
            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                AND TestTypeID = @TestTypeID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = localDrivingLicenseApplicationId;
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testTypeId;

                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    isFound = insertedId != -1;
                }
            }

            return isFound;
        }

        public static int GetNumberOfTestTrials(int localDrivingLicenseApplicationId, int testTypeId)
        {
            int numberOfTrials = 0;
            const string query = @"
            SELECT COUNT(*) 
            FROM TestAppointments
            WHERE TestTypeID = @TestTypeID 
                AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testTypeId;
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = 
                    localDrivingLicenseApplicationId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    numberOfTrials = Convert.ToInt32(result);
                }
            }

            return numberOfTrials;
        }

        public static bool GetTestResult(int localDrivingLicenseApplicationId, int testType)
        {
            bool testResult = false;

            const string query = @"
            SELECT t.TestResult
            FROM TestAppointments ta INNER JOIN Tests t 
	            ON ta.TestAppointmentID = t.TestAppointmentID 
            WHERE ta.TestTypeID = @TestTypeID 
	            AND ta.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = testType;
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value =
                    localDrivingLicenseApplicationId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    testResult = Convert.ToBoolean(result);
                }
            }

            return testResult;
        }
    }
}
