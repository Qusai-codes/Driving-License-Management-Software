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
    public class InternationalLicenseData
    {
        public static bool GetInternationalDrivingLicenseInfoById(int internationalLicenseId,
            ref int applicationId, ref int driverId, ref int issuedUsingLocalLicenseId,
            ref DateTime issueDate, ref DateTime expirationDate, ref bool isActive,
            ref int createdByUserId)
        {
            bool isFound = false;

            const string query = @"
            SELECT ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate,
                   ExpirationDate, IsActive, CreatedByUserID
            FROM InternationalLicenses
            WHERE InternationalLicenseID = @InternationalLicenseID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@InternationalLicenseID", SqlDbType.Int).Value = internationalLicenseId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        applicationId = (int)reader["ApplicationID"];
                        driverId = (int)reader["DriverID"];
                        issuedUsingLocalLicenseId = (int)reader["IssuedUsingLocalLicenseID"];
                        issueDate = (DateTime)reader["IssueDate"];
                        expirationDate = (DateTime)reader["ExpirationDate"];
                        isActive = (bool)reader["IsActive"];
                        createdByUserId = (int)reader["CreatedByUserID"];
                    }
                }
            }

            return isFound;
        }

        public static int AddNewInternationalDrivingLicense(
            int applicationId,
            int driverId,
            int issuedUsingLocalLicenseId,
            DateTime issueDate,
            DateTime expirationDate,
            bool isActive,
            int createdByUserId)
        {
            int internationalDrivingLicenseId = -1;

            const string query = @"
            INSERT INTO InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID, 
                IssueDate, ExpirationDate, IsActive, CreatedByUserID)
            VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, 
                @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;
                command.Parameters.Add("@IssuedUsingLocalLicenseID", SqlDbType.Int).Value = issuedUsingLocalLicenseId;
                command.Parameters.Add("@IssueDate", SqlDbType.SmallDateTime).Value = issueDate;
                command.Parameters.Add("@ExpirationDate", SqlDbType.SmallDateTime).Value = expirationDate;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    internationalDrivingLicenseId = Convert.ToInt32(result);
                }
            }

            return internationalDrivingLicenseId;
        }

        public static bool UpdateInternationalDrivingLicense(int interLicenseId, 
            bool isActive)
        {
            int rowsAffected = 0;

            const string query = @"
            UPDATE InternationalLicenses
            SET IsActive = @IsActive
            WHERE InternationalLicenseID = @InternationalLicenseID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@InternationalLicenseID", SqlDbType.Int).Value = interLicenseId;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllInternationalDrivingLicenses()
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                    IssueDate, ExpirationDate, IsActive, CreatedByUserID
            FROM InternationalLicenses;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static DataTable GetInternationalDrivingLicense(int driverId)
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID,
                   IssueDate, ExpirationDate, IsActive
            FROM InternationalLicenses
            WHERE DriverID = @DriverID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static bool DoesActiveInternationalLicenseExistForDriver(int driverId)
        {
            bool exists = false;

            const string query = @"
            SELECT TOP 1 1
            FROM InternationalLicenses
            WHERE DriverID = @DriverID
              AND IsActive = 1
              AND ExpirationDate >= CAST(GETDATE() AS date);
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;

                connection.Open();
                object result = command.ExecuteScalar();
                exists = result != null && result != DBNull.Value;
            }

            return exists;
        }

        public static bool DoesActiveInternationalLicenseExistForLocalLicenseId(int localLicenseId)
        {
            bool exists = false;

            const string query = @"
            SELECT TOP 1 1
            FROM InternationalLicenses
            WHERE IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID
              AND IsActive = 1
              AND ExpirationDate >= CAST(GETDATE() AS date);
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@IssuedUsingLocalLicenseID", SqlDbType.Int).Value = localLicenseId;

                connection.Open();
                object result = command.ExecuteScalar();
                exists = result != null && result != DBNull.Value;
            }

            return exists;
        }
    }
}
