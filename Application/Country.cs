using Business.Common;
using DataAccess.Data;
using System.Data;

namespace Business
{
    public class Country
    {
        // Note: no properties because Countries table is read only.
        public static DataTable GetAllCountries()
        {
            return CountryData.GetAllCountries();
        }

        public static string GetCountryNameById(int countryId)
        {
            return CountryData.GetCountryNameById(countryId);
        }
    }
}
