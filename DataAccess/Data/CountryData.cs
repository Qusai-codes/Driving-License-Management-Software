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
    }
}
