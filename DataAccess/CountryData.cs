using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CountryData
    {

        public static bool GetCountryInfoById(int id, out string countryName)
        {
            countryName = string.Empty;

            string query = "SELECT CountryName FROM Countries WHERE CountryID = @CountryID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = id;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                countryName = reader["CountryName"].ToString();
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return false;
        }

        public static bool GetCountryInfoByName(string countryName, out int id)
        {
            id = -1;

            string query = "SELECT CountryID FROM Countries WHERE CountryName = @CountryName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = countryName;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                id = (int)reader["CountryID"];
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public static DataTable GetAllCountries()
        {
            DataTable table = new DataTable();
            string query = "SELECT * FROM Countries ORDER BY CountryName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                table.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return table;
            }
            return table;
        }
    }
}
