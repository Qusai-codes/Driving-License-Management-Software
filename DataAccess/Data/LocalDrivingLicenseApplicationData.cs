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
                LocalDrivingLicenseApplicationID, 
                ApplicationID, 
                LicenseClassID 
            FROM LocalDrivingLicenseApplications;
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
    }
}
