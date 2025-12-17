using System.Configuration;

namespace DataAccess.Common
{
    internal static class DataAccessSettings
    {
        internal static string ConnectionString =>
            ConfigurationManager
                .ConnectionStrings["MyDatabaseConnection"]
                ?.ConnectionString
                ?? throw new ConfigurationErrorsException(
                    "Connection string 'MyDatabaseConnection' not found.");
    }
}
