using Contracts.DTOs;
using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class UserData
    {
        public static UserDto GetUserByUserId(int userId)
        {
            UserDto user = null;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                                SELECT 
	                                UserID,
	                                PersonID,
	                                UserName,
	                                Password,
	                                IsActive 
                                FROM Users WHERE UserID = @UserId;
                                ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = userId;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return null;
                            }
                            user = new UserDto
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                                PersonId = reader.GetInt32(reader.GetOrdinal("PersonID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

                return user;
        }

        public static int AddNewUser(UserDto user)
        {
            int userId = -1;

            string query = @"
                        INSERT INTO Users (PersonID, UserName, Password, IsActive)
                        VALUES (@PersonID, @UserName, @Password, @IsActive);

                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                    ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = user.PersonId;
                    command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 20).Value = user.UserName;
                    command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 20).Value = user.Password;
                    command.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = user.IsActive;

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }

            return userId;
        }

        public static bool UpdateUser(UserDto user)
        {
            return false;
        }

        public static bool DeleteUser(int userId)
        {
            return false;
        }

        public static bool IsUserExistsByPersonId(int personId)
        {
            string query = @"
                        SELECT CASE 
                            WHEN EXISTS (SELECT 1 FROM Users WHERE PersonID = @PersonID) 
                            THEN 1 ELSE 0 
                        END;
                    ";
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = personId;

                connection.Open();
                int result = (int)command.ExecuteScalar();

                return result == 1;
            }
        }

        public static bool IsUserExistsByUserId(int userId)
        {
            string query = @"
                        SELECT CASE 
                            WHEN EXISTS (SELECT 1 FROM Users WHERE UserID = @UserID) 
                            THEN 1 ELSE 0 
                        END;
                    ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserID", System.Data.SqlDbType.Int).Value = userId;

                connection.Open();
                int result = (int)command.ExecuteScalar();

                return result == 1;
            }
        }

        public static List<UserDto> GetAllUsers()
        {
            List<UserDto> users = new List<UserDto>();

            string query = @"
                            SELECT 
                                UserID, PersonID, UserName, IsActive 
                            FROM Users;
                            ";
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new UserDto
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    PersonId = reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    Password = null
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }

                return users;
        }
    }

    
}
