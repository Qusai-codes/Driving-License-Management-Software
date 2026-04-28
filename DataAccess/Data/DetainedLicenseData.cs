using DataAccess.Common;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class DetainedLicenseData
    {
        public static bool GetLicenseDetainInfoByLicenseId(int licenseId, ref int detainId, ref DateTime detainDate,
            ref decimal fineFees, ref int createdByUserId, ref bool isReleased, ref DateTime releaseDate,
            ref int releasedByUserId, ref int releaseApplicationId)
        {
            bool isFound = false;

            const string query = @"
            SELECT TOP 1 DetainID, DetainDate, FineFees, CreatedByUserID, 
                   IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID
            FROM DetainedLicenses 
            WHERE LicenseID = @LicenseID
            ORDER BY IsReleased ASC, DetainID DESC;
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

                        detainId = (int)reader["DetainID"];
                        detainDate = (DateTime)reader["DetainDate"];
                        fineFees = (decimal)reader["FineFees"];
                        createdByUserId = (int)reader["CreatedByUserID"];
                        isReleased = (bool)reader["IsReleased"];

                        releaseDate = reader["ReleaseDate"] == DBNull.Value
                            ? DateTime.MinValue
                            : (DateTime)reader["ReleaseDate"];

                        releasedByUserId = reader["ReleasedByUserID"] == DBNull.Value
                            ? -1
                            : (int)reader["ReleasedByUserID"];

                        releaseApplicationId = reader["ReleaseApplicationID"] == DBNull.Value
                            ? -1
                            : (int)reader["ReleaseApplicationID"];
                    }
                }
            }

            return isFound;
        }

        public static int AddNewDetainedDriverLicense(int licenseId, DateTime detainDate, 
            decimal fineFees, int createdByUserId, bool isReleased, DateTime releaseDate,
            int releasedByUserId, int releaseApplicationId)
        {
            int detainedId = -1;

            const string query = @"
            INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, 
                IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID)
            VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 
                @IsReleased, @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID);
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = licenseId;
                command.Parameters.Add("@DetainDate", SqlDbType.SmallDateTime).Value = detainDate;
                command.Parameters.Add("@FineFees", SqlDbType.SmallMoney).Value = fineFees;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserId;
                command.Parameters.Add("@IsReleased", SqlDbType.Bit).Value = isReleased;

                SqlParameter releaseDateParam = command.Parameters.Add("@ReleaseDate", SqlDbType.SmallDateTime);
                releaseDateParam.Value = releaseDate == DateTime.MinValue ? (object)DBNull.Value : releaseDate;

                SqlParameter releasedByParam = command.Parameters.Add("@ReleasedByUserID", SqlDbType.Int);
                releasedByParam.Value = releasedByUserId <= 0 ? (object)DBNull.Value : releasedByUserId;

                SqlParameter releaseAppParam = command.Parameters.Add("@ReleaseApplicationID", SqlDbType.Int);
                releaseAppParam.Value = releaseApplicationId <= 0 ? (object)DBNull.Value : releaseApplicationId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    detainedId = Convert.ToInt32(result);
                }
            }

            return detainedId;
        }

        public static bool UpdateDetainedLicense(int detainedLicenseId, bool isReleased,
            DateTime releaseDate, int releasedByUserId, int releaseApplicationId)
        {
            int rowsAffected = 0;

            const string query = @"
            UPDATE DetainedLicenses
            SET
                IsReleased = @IsReleased,
                ReleaseDate = @ReleaseDate,
                ReleasedByUserID = @ReleasedByUserID,
                ReleaseApplicationID = @ReleaseApplicationID
            WHERE DetainID = @DetainID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DetainID", SqlDbType.Int).Value = detainedLicenseId;
                command.Parameters.Add("@IsReleased", SqlDbType.Bit).Value = isReleased;

                SqlParameter releaseDateParam = command.Parameters.Add("@ReleaseDate", SqlDbType.SmallDateTime);
                releaseDateParam.Value = !isReleased || releaseDate == DateTime.MinValue
                    ? (object)DBNull.Value
                    : releaseDate;

                SqlParameter releasedByParam = command.Parameters.Add("@ReleasedByUserID", SqlDbType.Int);
                releasedByParam.Value = !isReleased || releasedByUserId <= 0
                    ? (object)DBNull.Value
                    : releasedByUserId;

                SqlParameter releaseAppParam = command.Parameters.Add("@ReleaseApplicationID", SqlDbType.Int);
                releaseAppParam.Value = !isReleased || releaseApplicationId <= 0
                    ? (object)DBNull.Value
                    : releaseApplicationId;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static bool IsLicenseDetained(int licenseId)
        {
            bool licenseIsDetained = false;

            // Hardcode IsReleased = 0 to check if it is CURRENTLY detained
            const string query = @"
            SELECT TOP 1 1 FROM DetainedLicenses 
            WHERE LicenseID = @LicenseID AND IsReleased = 0;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = licenseId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    licenseIsDetained = true;
                }
            }

            return licenseIsDetained;
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT 
                DetainID,
                LicenseID,
                DetainDate,
                FineFees,
                CreatedByUserID,
                IsReleased,
                ReleaseDate,
                ReleasedByUserID,
                ReleaseApplicationID
            FROM DetainedLicenses;
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
    }
}
