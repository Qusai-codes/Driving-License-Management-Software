using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public int PersonId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public static DataTable ToDataTable(IEnumerable<UserDto> users)
        {
            DataTable dt = GetDataTable();

            foreach (var user in users)
            {
                DataRow row = dt.NewRow();

                row["UserId"] = user.UserId;
                row["PersonId"] = user.PersonId;
                row["UserName"] = user.UserName;
                row["IsActive"] = user.IsActive;
                // No PasswordHash and PasswordSalt returned
                // for security.

                dt.Rows.Add(row);
            }

            return dt;
        }

        private static DataTable GetDataTable()
        {
            DataTable dt = new DataTable("User");

            dt.Columns.Add("UserId", typeof(int));
            dt.Columns.Add("PersonId", typeof(int));
            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("IsActive", typeof(bool));

            return dt;
        }

    }
}
