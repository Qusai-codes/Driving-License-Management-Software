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
        public static int AddNewDetainedDriverLicense(int licenseId, DateTime detainDate, 
            decimal fineFees, int createdByUserId, bool isReleased, DateTime releaseDate,
            int releasedByUserId, int releaseApplicationId)
        {
            int detainedLicenseId = -1;

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
                    detainedLicenseId = Convert.ToInt32(result);
                }
            }

            return detainedLicenseId;
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
    }
}
