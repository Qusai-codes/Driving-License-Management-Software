using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class DriverData
    {
        public static int AddNewDriver(int personId, int createdByUserId, 
            DateTime createdDate)
        {
            int driverId = -1;

            const string query = @"
            INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate) 
            VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserId;
                command.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = createdDate;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    driverId = insertedId;
                }
            }

            return driverId;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT DriverID, PersonID, CreatedByUserID, CreatedDate 
            FROM Drivers;
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

        public static bool GetDriverInfoById(int driverId, ref int personId, 
            ref int createdByUserId, ref DateTime createdDate)
        {
            bool isFound = false;

            const string query = @"
            SELECT PersonID, CreatedByUserID, CreatedDate 
            FROM Drivers 
            WHERE DriverID = @DriverID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        personId = (int)reader["PersonID"];
                        createdByUserId = (int)reader["CreatedByUserID"];
                        createdDate = (DateTime)reader["CreatedDate"];
                    }
                }
            }

            return isFound;
        }
    }
}
