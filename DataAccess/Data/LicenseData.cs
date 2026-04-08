using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class LicenseData
    {
        public static int AddNewLicense(int applicationId, int driverId, 
            int licenseClassId, DateTime issueDate, DateTime expirationDate, 
            string notes, decimal paidFees, bool isActive, byte issueReason,
            int createdByUserId)
        {
            int licenseId = -1;

            const string query = @"
            INSERT INTO Licenses (ApplicationID, DriverID, LicenseClass, 
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, 
                IssueReason, CreatedByUserID) 
            VALUES (@ApplicationID, @DriverID, @LicenseClass, 
                @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, 
                @IssueReason, @CreatedByUserID);
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = driverId;
                command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = licenseClassId;
                command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = issueDate;
                command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = expirationDate;

                SqlParameter notesParam = command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500);
                notesParam.Value = string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes;

                command.Parameters.Add("@PaidFees", SqlDbType.SmallMoney).Value = paidFees;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                command.Parameters.Add("@IssueReason", SqlDbType.TinyInt).Value = issueReason;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    licenseId = insertedID;
                }
            }

            return licenseId;
        }
    }
}
