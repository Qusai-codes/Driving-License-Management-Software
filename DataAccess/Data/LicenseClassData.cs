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

        public static byte GetDefaultValidityLength(int licenseClassId)
        {
            byte minimumAllowedAge = 0;

            const string query = @"
                SELECT MinimumAllowedAge
                FROM LicenseClasses
                WHERE LicenseClassID = @LicenseClassID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = licenseClassId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    minimumAllowedAge = Convert.ToByte(result);
                }
            }

            return minimumAllowedAge;
        }

        public static string GetLicenseClassName(int licenseClassId)
        {
            string className = string.Empty;

            const string query = @"
                SELECT ClassName
                FROM LicenseClasses
                WHERE LicenseClassID = @LicenseClassID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = licenseClassId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    className = result.ToString();
                }
            }

            return className;
        }

        public static byte GetMinimumAllowedAge(int licenseClassId)
        {
            byte minimumAllowedAge = 0;

            const string query = @"
                SELECT MinimumAllowedAge
                FROM LicenseClasses
                WHERE LicenseClassID = @LicenseClassID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = licenseClassId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    minimumAllowedAge = Convert.ToByte(result);
                }
            }

            return minimumAllowedAge;
        }
    }
}
