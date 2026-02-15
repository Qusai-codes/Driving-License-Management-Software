using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common;
using Contracts.DTOs;

namespace DataAccess.Data
{
    public class CountryData
    {

        public static List<CountryDto> GetAllCountries()
        {
            List<CountryDto> countries = new List<CountryDto>();
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = "SELECT CountryID, CountryName FROM Countries;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CountryDto country = new CountryDto
                                {
                                    CountryId = Convert.ToInt32(reader["CountryID"]),
                                    CountryName = reader["CountryName"].ToString()
                                };
                                countries.Add(country);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return countries;
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
