using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Data
{
    public class TestData
    {

        public static bool GetTestInfoById(int testId, ref int testAppointmentId,
            ref bool testResult, ref string notes, ref int createdByUserId)
        {
            bool isFound = false;

            const string query = @"
            SELECT TestAppointmentID, TestResult, Notes, CreatedByUserID
            FROM Tests
            WHERE TestID = @TestID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestID", SqlDbType.Int).Value = testId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        testAppointmentId = (int)reader["TestAppointmentID"];
                        testResult = (bool)reader["TestResult"];
                        notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();
                        createdByUserId = (int)reader["CreatedByUserID"];
                    }
                }
            }

            return isFound;
        }

        public static int AddNewTest(int testAppointmentId, bool testResult, 
            string notes, int createdByUserId)
        {
            int testId = -1;

            const string query = @"
            INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID) 
            VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID); 
            SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = testAppointmentId;
                command.Parameters.Add("@TestResult", SqlDbType.Bit).Value = testResult;

                SqlParameter notesParam = command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500);
                notesParam.Value = string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes;

                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    testId = insertedID;
                }
            }

            return testId;
        }

        
    }
}
