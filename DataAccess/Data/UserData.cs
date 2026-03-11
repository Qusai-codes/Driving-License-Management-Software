using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Contracts.DTOs;
using DataAccess.Common;

namespace DataAccess.Data
{
    public static class UserData
    {

        public static int AddNewUser(UserDto user)
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
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = user.PersonId;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = user.UserName;
                command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 200).Value = user.PasswordHash;
                command.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 200).Value = user.PasswordSalt;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    userId = insertedId;
                }
            }
            return userId;
        }

        public static UserDto GetUserByUserName(string userName)
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
                        return null;

                    return new UserDto
                    {
                        UserId = (int)reader["UserID"],
                        PersonId = (int)reader["PersonID"],
                        UserName = (string)reader["UserName"],
                        IsActive = (bool)reader["IsActive"],
                        PasswordHash = (string)reader["PasswordHash"],
                        PasswordSalt = (string)reader["PasswordSalt"]
                    };
                }
            }
        }

        public static UserDto GetUserByUserId(int userId)
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
                        return null;

                    return new UserDto
                    {
                        UserId = (int)reader["UserID"],
                        PersonId = (int)reader["PersonID"],
                        UserName = (string)reader["UserName"],
                        IsActive = (bool)reader["IsActive"],
                        PasswordHash = (string)reader["PasswordHash"],
                        PasswordSalt = (string)reader["PasswordSalt"]
                    };
                }
            }
        }

        public static List<UserDto> GetAllUsers()
        {
            const string query = @"
                SELECT UserID, PersonID, UserName, IsActive
                FROM dbo.Users;
            ";

            List<UserDto> users = new List<UserDto>();

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int ordUserId = reader.GetOrdinal("UserID");
                    int ordPersonId = reader.GetOrdinal("PersonID");
                    int ordUserName = reader.GetOrdinal("UserName");
                    int ordIsActive = reader.GetOrdinal("IsActive");

                    while (reader.Read())
                    {
                        users.Add(new UserDto
                        {
                            UserId = reader.GetInt32(ordUserId),
                            PersonId = reader.GetInt32(ordPersonId),
                            UserName = reader.GetString(ordUserName),
                            IsActive = reader.GetBoolean(ordIsActive)
                        });
                    }
                }
            }

            return users;
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
        public static bool UpdateUser(UserDto user)
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
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = user.PersonId;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = user.UserName;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = user.UserId;

                connection.Open();
                rowsAffected =  command.ExecuteNonQuery();
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
                rowsAffected =  command.ExecuteNonQuery();
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
                rowsAffected =  command.ExecuteNonQuery();
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
                isFound =  command.ExecuteScalar() != null;
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
                isFound =  command.ExecuteScalar() != null;
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