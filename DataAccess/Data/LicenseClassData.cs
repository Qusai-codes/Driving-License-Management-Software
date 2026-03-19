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
    public class LicenseClassData
    {
        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            const string query = @"
            SELECT 
                LicenseClassID,
                ClassName
            FROM LicenseClasses
            ORDER BY LicenseClassID;";

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

        public static string[] GetAllLicenseClassNames()
        {
            List<string> classNames = new List<string>();

            const string query = @"
                SELECT ClassName
                FROM LicenseClasses
                ORDER BY LicenseClassID;";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classNames.Add(reader["ClassName"].ToString());
                    }
                }
            }

            return classNames.ToArray();
        }
    }
}
