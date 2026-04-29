using System;
using System.Data;
using System.Data.SqlClient;
using DataAccess.Common;

namespace DataAccess.Data
{
    public class CountryData
    {
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = "SELECT CountryID AS CountryId, CountryName FROM Countries;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
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
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return dt;
        }

        public static string GetCountryNameById(int countryId)
        {
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = "SELECT CountryName FROM Countries WHERE CountryID = @CountryID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CountryID", SqlDbType.Int).Value = countryId;

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        return result?.ToString();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
