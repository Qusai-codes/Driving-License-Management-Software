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


    }
}
