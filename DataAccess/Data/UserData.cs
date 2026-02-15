using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class UserData
    {
        public static bool IsUserExists(int personId)
        {
            bool found = false;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = "SELECT FOUND = 1 FROM Users WHERE PersonID = @PersonID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = personId;

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        found = result != null;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

                return found;
        }
    }
}
