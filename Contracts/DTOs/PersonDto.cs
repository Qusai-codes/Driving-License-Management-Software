using System;
using System.Data;
using System.Collections.Generic;

namespace Contracts.DTOs
{
    public class PersonDto
    {
        public int PersonId { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryId { get; set; }
        public string ImagePath { get; set; }

        // Schema only
        public static DataTable GetDataTable()
        {
            DataTable dt = new DataTable("Person");

            dt.Columns.Add("PersonId", typeof(int));
            dt.Columns.Add("NationalNo", typeof(string));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("SecondName", typeof(string));
            dt.Columns.Add("ThirdName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("DateOfBirth", typeof(DateTime));
            dt.Columns.Add("Gender", typeof(byte));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("NationalityCountryId", typeof(int));
            dt.Columns.Add("ImagePath", typeof(string));

            return dt;
        }

        // List<DTO> → DataTable
        public static DataTable ToDataTable(IEnumerable<PersonDto> people)
        {
            DataTable dt = GetDataTable();

            foreach (var p in people)
            {
                DataRow row = dt.NewRow();

                row["PersonId"] = p.PersonId;
                row["NationalNo"] = p.NationalNo;
                row["FirstName"] = p.FirstName;
                row["SecondName"] = p.SecondName;
                row["ThirdName"] = p.ThirdName ?? (object)DBNull.Value;
                row["LastName"] = p.LastName;
                row["DateOfBirth"] = p.DateOfBirth;
                row["Gender"] = p.Gender;
                row["Address"] = p.Address;
                row["Phone"] = p.Phone;
                row["Email"] = p.Email ?? (object)DBNull.Value;
                row["NationalityCountryId"] = p.NationalityCountryId;
                row["ImagePath"] = p.ImagePath ?? (object)DBNull.Value;

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}