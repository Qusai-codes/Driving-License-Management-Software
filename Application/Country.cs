using Business.Common;
using Contracts.DTOs;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Country
    {
        // Note: no properties because Countries table is read only.
        public static List<CountryDto> GetAllCountries()
        {
            return CountryData.GetAllCountries();
        }


    }
}
