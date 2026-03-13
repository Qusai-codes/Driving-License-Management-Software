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
    public class TestTypeData
    {

        public static bool GetTestTypeInfoById(int id, ref string title,
            ref string description, ref decimal fees)
        {
            bool isFound = false;
            const string query = @"
                SELECT 
                    TestTypeTitle,
                    TestTypeDescription,
                    TestTypeFees
                FROM TestTypes
                WHERE TestTypeID = @TestTypeID;
                ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = id;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                        title = (string)reader["TestTypeTitle"];
                        description = (string)reader["TestTypeDescription"];
                        fees = (decimal)reader["TestTypeFees"];
                    }
                    else
                    {
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool UpdateTestType(int id, string title, 
            string description, decimal fees)
        {
            int rowsAffected = 0;

            const string query = @"
                UPDATE TestTypes
                SET 
                    TestTypeTitle = @TestTypeTitle,
                    TestTypeDescription = @TestTypeDescription,
                    TestTypeFees = @TestTypeFees 
                WHERE TestTypeID = @TestTypeID;
                ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = id;
                command.Parameters.Add("@TestTypeTitle", SqlDbType.NVarChar, 100).Value = title;
                command.Parameters.Add("@TestTypeDescription", SqlDbType.NVarChar, 500).Value = description;
                command.Parameters.Add("@TestTypeFees", SqlDbType.SmallMoney).Value = fees;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            const string query = @"
                SELECT 
                    TestTypeID,
                    TestTypeTitle,
                    TestTypeDescription,
                    TestTypeFees
                FROM TestTypes;
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
