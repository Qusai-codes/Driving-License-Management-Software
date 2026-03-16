using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DataAccess.Common;

namespace DataAccess.Data
{
    public class ApplicationTypeData
    {

        public static bool GetApplicationTypeInfoById(int id, ref string title, 
            ref decimal fees)
        {
            bool isFound = false;

            const string query = @"
                SELECT ApplicationTypeTitle, ApplicationFees 
                FROM ApplicationTypes
                WHERE ApplicationTypeID = @ApplicationTypeID;
                ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = id;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                        title = (string)reader["ApplicationTypeTitle"];
                        fees = (decimal)reader["ApplicationFees"];
                    }
                    else
                    {
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            const string query = @"
                SELECT ApplicationTypeID, ApplicationTypeTitle, ApplicationFees 
                FROM ApplicationTypes;
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

        public static bool UpdateApplicationType(int id, string title, decimal fees)
        {
            int rowsAffected = 0;

            const string query = @"
                UPDATE ApplicationTypes 
                SET ApplicationTypeTitle = @ApplicationTypeTitle,
                    ApplicationFees = @ApplicationFees
                WHERE ApplicationTypeID = @ApplicationTypeID;
                ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = id;
                command.Parameters.Add("@ApplicationTypeTitle", SqlDbType.NVarChar, 150).Value = title;
                command.Parameters.Add("@ApplicationFees", SqlDbType.SmallMoney).Value = fees;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static decimal GetApplicationTypeFees(string applicationTypeTitle)
        {
            decimal fees = 0;

            const string query = @"
                SELECT ApplicationFees 
                FROM ApplicationTypes
                WHERE ApplicationTypeTitle = @ApplicationTypeTitle;
                ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationTypeTitle", SqlDbType.NVarChar, 150).Value = applicationTypeTitle;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        fees = (decimal)reader["ApplicationFees"];
                    }
                }
            }

            return fees;
        }
    }
}
