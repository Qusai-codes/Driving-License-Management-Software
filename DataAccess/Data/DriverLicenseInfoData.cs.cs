using DataAccess.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Data
{
    public class DriverLicenseInfoData
    {
        public static bool GetDriverLicenseInfoByDriverId(int driverId, ref int driverIdOut,
            ref string nationalNumber, ref string fullName, ref byte gender, ref DateTime dateOfBirth,
            ref string imagePath, ref int licenseClassId, ref int licenseId, ref DateTime issueDate,
            ref byte issueReason, ref string notes, ref bool isActive, ref bool isDetained,
            ref DateTime expirationDate)
        {
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
                    if (!reader.Read())
                        return false;

                    driverIdOut = (int)reader["DriverID"];
                    nationalNumber = reader["NationalNumber"].ToString();
                    fullName = reader["FullName"].ToString();
                    gender = (byte)reader["Gender"];
                    dateOfBirth = (DateTime)reader["DateOfBirth"];
                    imagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString();

                    licenseClassId = (int)reader["LicenseClassId"];
                    licenseId = (int)reader["LicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    issueReason = (byte)reader["IssueReason"];
                    notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();
                    isActive = (bool)reader["IsActive"];
                    isDetained = (bool)reader["IsDetained"];
                    expirationDate = (DateTime)reader["ExpirationDate"];

                    return true;
                }
            }
        }

        public static bool GetDriverLicenseInfoByLicenseId(int licenseId, ref int driverIdOut,
            ref string nationalNumber, ref string fullName, ref byte gender, ref DateTime dateOfBirth,
            ref string imagePath, ref int licenseClassId, ref int licenseIdOut, ref DateTime issueDate,
            ref byte issueReason, ref string notes, ref bool isActive, ref bool isDetained,
            ref DateTime expirationDate)
        {
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
            FROM Licenses l
            INNER JOIN Drivers d ON d.DriverID = l.DriverID
            INNER JOIN People p ON p.PersonID = d.PersonID
            WHERE l.LicenseID = @LicenseID;";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = licenseId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return false;

                    driverIdOut = (int)reader["DriverID"];
                    nationalNumber = reader["NationalNumber"].ToString();
                    fullName = reader["FullName"].ToString();
                    gender = (byte)reader["Gender"];
                    dateOfBirth = (DateTime)reader["DateOfBirth"];
                    imagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString();

                    licenseClassId = (int)reader["LicenseClassId"];
                    licenseIdOut = (int)reader["LicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    issueReason = (byte)reader["IssueReason"];
                    notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();
                    isActive = (bool)reader["IsActive"];
                    isDetained = (bool)reader["IsDetained"];
                    expirationDate = (DateTime)reader["ExpirationDate"];

                    return true;
                }
            }
        }
    }
}
