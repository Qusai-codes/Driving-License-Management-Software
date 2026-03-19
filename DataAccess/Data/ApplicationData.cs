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
    public class ApplicationData
    {

        public static bool GetApplicationInfoById(int id, ref int personId, ref DateTime applicationDate,
            ref int applicationTypeId, ref byte applicationStatus, ref DateTime lastStatusDate,
            ref decimal paidFees, ref int userId)
        {
            bool isFound = false;
            const string query = @"
            SELECT 
	            ApplicationID, ApplicantPersonID, ApplicationDate, 
	            ApplicationTypeID, ApplicationStatus, LastStatusDate,
	            PaidFees, CreatedByUserID
            FROM Applications 
            WHERE ApplicationID = @ApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = id;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        personId = (int)reader["ApplicantPersonID"];
                        applicationDate = (DateTime)reader["ApplicationDate"];
                        applicationTypeId = (int)reader["ApplicationTypeID"];
                        applicationStatus = (byte)reader["ApplicationStatus"];
                        lastStatusDate = (DateTime)reader["LastStatusDate"];
                        paidFees = (decimal)reader["PaidFees"];
                        userId = (int)reader["CreatedByUserID"];

                    }
                }
            }

            return isFound;
        }

        public static int AddNewApplication(int personId, DateTime applicationDate, 
            int applicationTypeId, byte applicationStatus, DateTime lastStatusDate, 
            decimal paidFees, int userId)
        {
            int applicationId = -1;

            const string query = @"
                INSERT INTO Applications 
                    (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus,
                    LastStatusDate, PaidFees, CreatedByUserID) 
                VALUES (@PersonId, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus,
                        @LastStatusDate, @PaidFees, @UserId);
                SELECT SCOPE_IDENTITY();
                ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonId", SqlDbType.Int).Value = personId;
                command.Parameters.Add("@ApplicationDate", SqlDbType.DateTime).Value = applicationDate;
                command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = applicationTypeId;
                command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = applicationStatus;
                command.Parameters.Add("@LastStatusDate", SqlDbType.DateTime).Value = lastStatusDate;
                command.Parameters.Add("@PaidFees", SqlDbType.SmallMoney).Value = paidFees;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    applicationId = insertedID;
                }
            }

            return applicationId;
        }

        public static bool UpdateApplication(int applicationId, byte applicationStatus, 
            DateTime lastStatusDate)
        {
            int rowsAffected = 0;

            const string query = @"
            UPDATE Applications 
            SET 
	            ApplicationStatus = @ApplicationStatus,
	            LastStatusDate = @LastStatusDate
            WHERE ApplicationID = @ApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;
                command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = applicationStatus;
                command.Parameters.Add("@LastStatusDate", SqlDbType.DateTime).Value = lastStatusDate;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT 
	            ApplicationID, ApplicantPersonID, ApplicationDate, 
	            ApplicationTypeID, ApplicationStatus, LastStatusDate, 
	            PaidFees, CreatedByUserID
            FROM Applications;
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

        public static bool DeleteApplication(int applicationId)
        {
            int rowsAffected = 0;

            const string query = @"
            DELETE FROM Applications 
            WHERE ApplicationID = @ApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }

        public static byte GetApplicationStatus(int applicationId)
        {
            byte status = 0;

            const string query = @"
            SELECT ApplicationStatus
            FROM Applications
            WHERE ApplicationID = @ApplicationID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    status = Convert.ToByte(result);
                }
            }

            return status;
        }
    }
}
