using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class LocalDrivingLicenseApplicationData
    {

        public static bool GetApplicationInfoById(int localDrivingLicenseApplicationId, 
            ref int applicationId, ref int licenseClassId)
        {
            bool isFound = false;

            const string query = @"
            SELECT * FROM LocalDrivingLicenseApplications 
            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID",
                    SqlDbType.Int).Value = localDrivingLicenseApplicationId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        applicationId = (int)reader["ApplicationID"];
                        licenseClassId = (int)reader["LicenseClassID"];
                    }
                    else
                    {
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static int AddNewApplication(int applicationId, 
            int licenseClassId)
        {
            int localDrivingLicenseApplicationId = -1;

            const string query = @"
            INSERT INTO LocalDrivingLicenseApplications 
	            (ApplicationID, LicenseClassID)
	            VALUES (@ApplicationID, @LicenseClassID);
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = licenseClassId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    localDrivingLicenseApplicationId = insertedID;
                }
            }

            return localDrivingLicenseApplicationId;
        }

        public static bool DeleteApplication(int applicationId)
        {
            int rowsAffected = 0;

            const string query = @"
            DELETE FROM LocalDrivingLicenseApplications 
            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT 
                l.LocalDrivingLicenseApplicationID,
                lc.ClassName,
                p.NationalNo,

                p.FirstName
                    + ' ' + p.SecondName
                    + CASE 
                        WHEN p.ThirdName IS NULL OR p.ThirdName = '' 
                            THEN '' 
                        ELSE ' ' + p.ThirdName 
                      END
                    + ' ' + p.LastName AS FullName,

                ap.ApplicationDate,

                (
                    SELECT COUNT(*)
                    FROM Tests t
                    INNER JOIN TestAppointments ta 
                        ON t.TestAppointmentID = ta.TestAppointmentID
                    WHERE ta.LocalDrivingLicenseApplicationID = l.LocalDrivingLicenseApplicationID
                      AND t.TestResult = 1
                ) AS PassedTests,
                ap.ApplicationStatus

            FROM LocalDrivingLicenseApplications l
            INNER JOIN LicenseClasses lc 
                ON l.LicenseClassID = lc.LicenseClassID
            INNER JOIN Applications ap 
                ON l.ApplicationID = ap.ApplicationID
            INNER JOIN People p 
                ON p.PersonID = ap.ApplicantPersonID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }

        public static int GetApplicationId(int personId,
            int licenseClassId, byte applicationStatus)
        {
            int applicationId = -1;

            const string query = @"
            SELECT ap.ApplicationID 
            FROM LocalDrivingLicenseApplications loc 
            INNER JOIN Applications ap ON loc.ApplicationID = ap.ApplicationID 
            WHERE ap.ApplicantPersonID = @ApplicantPersonID 
	            AND loc.LicenseClassID = @LicenseClassID 
	            AND ap.ApplicationStatus = @ApplicationStatus;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = personId;
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = licenseClassId;
                command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = applicationStatus;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    applicationId = Convert.ToInt32(result);
                }
            }

            return applicationId;
        }

        public static int GetApplicationId(int localApplicationId)
        {
            int applicationId = -1;

            const string query = @"
            SELECT ApplicationID 
            FROM LocalDrivingLicenseApplications 
            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = localApplicationId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    applicationId = Convert.ToInt32(result);
                }
            }

            return applicationId;
        }

        public static DataTable GetTestsTaken(int localDrivingLicenseApplicationId)
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT 
                t.TestID,
                t.TestAppointmentID,
                t.TestResult,
                t.Notes
            FROM Tests t
            INNER JOIN TestAppointments ta
                ON t.TestAppointmentID = ta.TestAppointmentID
            WHERE ta.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
                    .Value = localDrivingLicenseApplicationId;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }
    }
}
