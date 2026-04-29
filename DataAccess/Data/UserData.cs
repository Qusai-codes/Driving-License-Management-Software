using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataAccess.Common;

namespace DataAccess.Data
{
    public static class UserData
    {
        public static int AddNewUser(int personId, string userName, string passwordHash,
            string passwordSalt, bool isActive)
        {
            int userId = -1;
            const string query = @"
                INSERT INTO dbo.Users (PersonID, UserName, PasswordHash, PasswordSalt, IsActive)
                VALUES (@PersonID, @UserName, @PasswordHash, @PasswordSalt, @IsActive);

                SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;
                command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 200).Value = passwordHash;
                command.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 200).Value = passwordSalt;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    userId = insertedId;
                }
            }
            return userId;
        }

        public static bool GetUserByUserName(string userName, ref int userId, ref int personId,
            ref string dbUserName, ref bool isActive, ref string passwordHash, ref string passwordSalt)
        {
            const string query = @"
                SELECT UserID, PersonID, UserName, IsActive, PasswordHash, PasswordSalt
                FROM dbo.Users
                WHERE UserName = @UserName;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return false;

                    userId = (int)reader["UserID"];
                    personId = (int)reader["PersonID"];
                    dbUserName = (string)reader["UserName"];
                    isActive = (bool)reader["IsActive"];
                    passwordHash = (string)reader["PasswordHash"];
                    passwordSalt = (string)reader["PasswordSalt"];
                    return true;
                }
            }
        }

        public static bool GetUserByUserId(int userId, ref int personId, ref string userName,
            ref bool isActive, ref string passwordHash, ref string passwordSalt)
        {
            const string query = @"
                SELECT UserID, PersonID, UserName, IsActive, PasswordHash, PasswordSalt
                FROM dbo.Users
                WHERE UserID = @UserID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return false;

                    personId = (int)reader["PersonID"];
                    userName = (string)reader["UserName"];
                    isActive = (bool)reader["IsActive"];
                    passwordHash = (string)reader["PasswordHash"];
                    passwordSalt = (string)reader["PasswordSalt"];
                    return true;
                }
            }
        }

        public static DataTable GetAllUsers()
        {
            const string query = @"
                SELECT UserID AS UserId, PersonID AS PersonId, UserName, IsActive
                FROM dbo.Users;
            ";

            DataTable dt = new DataTable();

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }

        public static (string Hash, string Salt) GetPasswordData(string userName)
        {
            const string query = @"
                SELECT PasswordHash, PasswordSalt
                FROM dbo.Users
                WHERE UserName = @UserName;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return (null, null);

                    return (
                        reader["PasswordHash"].ToString(),
                        reader["PasswordSalt"].ToString()
                    );
                }
            }
        }

        public static bool UpdateUser(int userId, int personId, string userName, bool isActive)
        {
            int rowsAffected = 0;
            const string query = @"
                UPDATE dbo.Users
                SET PersonID = @PersonID,
                    UserName = @UserName,
                    IsActive = @IsActive
                WHERE UserID = @UserID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }

        public static bool UpdateUserPassword(int userId, string newHash, string newSalt)
        {
            int rowsAffected = 0;
            const string query = @"
                UPDATE dbo.Users
                SET PasswordHash = @Hash,
                    PasswordSalt = @Salt
                WHERE UserID = @UserID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Hash", SqlDbType.NVarChar, 200).Value = newHash;
                command.Parameters.Add("@Salt", SqlDbType.NVarChar, 200).Value = newSalt;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }

        public static bool DeleteUser(int userId)
        {
            int rowsAffected = 0;
            const string query = "DELETE FROM dbo.Users WHERE UserID = @UserID;";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }

        public static int GetPersonId(int userId)
        {
            int personId = -1;

            const string query = @"
                SELECT PersonID
                FROM dbo.Users
                WHERE UserID = @UserID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    personId = Convert.ToInt32(result);
                }

            }

            return personId;
        }

        public static bool IsUserExistsByPersonId(int personId)
        {
            bool isFound = false;
            const string query = @"
                SELECT 1
                FROM dbo.Users
                WHERE PersonID = @PersonID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;

                connection.Open();
                isFound = command.ExecuteScalar() != null;
            }
            return isFound;
        }

        public static bool IsUserExistsByUserId(int userId)
        {
            bool isFound = false;
            const string query = @"
                SELECT 1
                FROM dbo.Users
                WHERE UserID = @UserID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                connection.Open();
                isFound = command.ExecuteScalar() != null;
            }
            return isFound;
        }

        public static bool IsUserActive(string userName)
        {
            bool isActive = false;
            const string query = @"
                SELECT 1
                FROM dbo.Users
                WHERE UserName = @UserName AND IsActive = 1;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;

                connection.Open();
                isActive = command.ExecuteScalar() != null;
            }
            return isActive;
        }

        public static bool IsUserExistByUserName(string userName)
        {
            bool isFound = false;
            const string query = @"
                SELECT 1
                FROM dbo.Users
                WHERE UserName = @UserName;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;

                connection.Open();
                isFound = command.ExecuteScalar() != null;
            }
            return isFound;
        }

        public static bool HasUsers()
        {
            bool hasUsers = false;
            const string query = @"
                SELECT TOP 1 1
                FROM dbo.Users;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                hasUsers = command.ExecuteScalar() != null;
            }
            return hasUsers;
        }
    }
}