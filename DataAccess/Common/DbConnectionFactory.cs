using System.Data.SqlClient;

namespace DataAccess.Common
{
    internal static class DbConnectionFactory
    {
        internal static SqlConnection CreateConnection()
        {
            return new SqlConnection(DataAccessSettings.ConnectionString);
        }
    }
}
