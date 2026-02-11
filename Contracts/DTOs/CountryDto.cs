using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Contracts.DTOs
{
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public static DataTable GetDataTable()
        {
            DataTable dt = new DataTable("Country");

            dt.Columns.Add("CountryId", typeof(int));
            dt.Columns.Add("CountryName", typeof(string));

            return dt;
        }

        public static DataTable ToDataTable(IEnumerable<CountryDto> countries)
        {
            DataTable dt = GetDataTable();

            foreach (var c in countries)
            {
                DataRow row = dt.NewRow();

                row["CountryId"] = c.CountryId;
                row["CountryName"] = c.CountryName;

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
