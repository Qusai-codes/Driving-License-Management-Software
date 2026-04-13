using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Data
{
    public class LicenseData
    {
        public static bool GetLicenseInfoById(int licenseId, ref int applicationId, ref int driverId,
            ref int licenseClassId, ref DateTime issueDate, ref DateTime expirationDate,
            ref string notes, ref decimal paidFees, ref bool isActive, ref byte issueReason,
            ref int createdByUserId)
        {
            bool isFound = false;

            const string query = @"
            SELECT ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate,
                   Notes, PaidFees, IsActive, IssueReason, CreatedByUserID
            FROM Licenses
            WHERE LicenseID = @LicenseID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = licenseId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        applicationId = (int)reader["ApplicationID"];
                        driverId = (int)reader["DriverID"];
                        licenseClassId = (int)reader["LicenseClass"];
                        issueDate = (DateTime)reader["IssueDate"];
                        expirationDate = (DateTime)reader["ExpirationDate"];
                        notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();
                        paidFees = (decimal)reader["PaidFees"];
                        isActive = (bool)reader["IsActive"];
                        issueReason = (byte)reader["IssueReason"];
                        createdByUserId = (int)reader["CreatedByUserID"];
                    }
                }
            }

            return isFound;
        }

        public static int AddNewLicense(int applicationId, int driverId, 
            int licenseClassId, DateTime issueDate, DateTime expirationDate, 
            string notes, decimal paidFees, bool isActive, byte issueReason,
            int createdByUserId)
        {
            int licenseId = -1;

            const string query = @"
            INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, 
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, 
                IssueReason, CreatedByUserID) 
            VALUES (@ApplicationID, @DriverID, @LicenseClass, 
                @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, 
                @IssueReason, @CreatedByUserID);
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;
                command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = licenseClassId;
                command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = issueDate;
                command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = expirationDate;

                SqlParameter notesParam = command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500);
                notesParam.Value = string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes;

                command.Parameters.Add("@PaidFees", SqlDbType.SmallMoney).Value = paidFees;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                command.Parameters.Add("@IssueReason", SqlDbType.TinyInt).Value = issueReason;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    licenseId = insertedID;
                }
            }

            return licenseId;
        }

        public static int GetLicenseIdByApplicationId(int applicationId)
        {
            int licenseId = -1;

            const string query = @"
            SELECT LicenseID FROM Licenses WHERE ApplicationID = @ApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    licenseId = Convert.ToInt32(result);
                }
            }

            return licenseId;
        }

        public static bool UpdateLicense(int licenseId, bool isActive)
        {
            int rowsAffected = 0;

            const string query = @"
            UPDATE Licenses 
            SET IsActive = @IsActive  
            WHERE LicenseID = @LicenseID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = licenseId;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static bool DoesActiveLicenseExistForDriverAndClass(int driverId, int licenseClassId)
        {
            bool exists = false;

            const string query = @"
            SELECT TOP 1 1
            FROM Licenses
            WHERE DriverID = @DriverID
              AND LicenseClass = @LicenseClass
              AND IsActive = 1;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;
                command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = licenseClassId;

                connection.Open();
                object result = command.ExecuteScalar();
                exists = result != null && result != DBNull.Value;
            }

            return exists;
        }

        public static bool DoesLicenseExist(int licenseId)
        {
            const string query = @"SELECT 1 FROM Licenses WHERE LicenseID = @LicenseID;";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = licenseId;

                connection.Open();
                object result = command.ExecuteScalar();
                return result != null && result != DBNull.Value;
            }
        }
    }
}
