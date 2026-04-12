using Contracts;
using Contracts.DTOs;
using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class DriverLicenseInfoData
    {
        public static DriverLicenseInfoDto GetDriverLicenseInfoByDriverId(int driverId)
        {
            DriverLicenseInfoDto driverLicenseInfo = null;

            const string query = @"
            SELECT TOP 1
                d.DriverID,
                p.NationalNo AS NationalNumber,
                p.FirstName + ' ' + p.SecondName
                    + CASE 
                        WHEN p.ThirdName IS NULL OR p.ThirdName = '' THEN ''
                        ELSE ' ' + p.ThirdName
                      END
                    + ' ' + p.LastName AS FullName,
                p.Gendor AS Gender,
                p.DateOfBirth,
                p.ImagePath,
                l.LicenseID,
                l.LicenseClass AS LicenseClassId,
                l.IssueDate,
                l.IssueReason,
                l.Notes,
                l.IsActive,
                l.ExpirationDate,
                CASE 
                    WHEN EXISTS (
                        SELECT 1
                        FROM DetainedLicenses dl
                        WHERE dl.LicenseID = l.LicenseID
                          AND dl.IsReleased = 0
                    ) THEN CAST(1 AS bit)
                    ELSE CAST(0 AS bit)
                END AS IsDetained
            FROM Drivers d
            INNER JOIN People p ON p.PersonID = d.PersonID
            INNER JOIN Licenses l ON l.DriverID = d.DriverID
            WHERE d.DriverID = @DriverID
            ORDER BY l.IsActive DESC, l.IssueDate DESC, l.LicenseID DESC;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        driverLicenseInfo = new DriverLicenseInfoDto
                        {
                            DriverId = (int)reader["DriverID"],
                            NationalNumber = reader["NationalNumber"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Gender = (byte)reader["Gender"],
                            DateOfBirth = (DateTime)reader["DateOfBirth"],
                            ImagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString(),

                            LicenseClassId = (int)reader["LicenseClassId"],
                            LicenseId = (int)reader["LicenseID"],
                            IssueDate = (DateTime)reader["IssueDate"],
                            IssueReason = (byte)reader["IssueReason"],
                            Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),
                            IsActive = (bool)reader["IsActive"],
                            IsDetained = (bool)reader["IsDetained"],
                            ExpirationDate = (DateTime)reader["ExpirationDate"]
                        };
                    }
                }
            }

            return driverLicenseInfo;
        }
    }
}
