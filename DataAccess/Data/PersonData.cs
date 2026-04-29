using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common;

namespace DataAccess.Data
{
    public class PersonData
    {
        public static bool GetPersonInfoById(int personId, ref string nationalNo, ref string firstName,
            ref string secondName, ref string thirdName, ref string lastName, ref DateTime dateOfBirth,
            ref byte gender, ref string address, ref string phone, ref string email,
            ref int nationalityCountryId, ref string imagePath)
        {
            bool isFound = false;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    SELECT
                        PersonID AS PersonId,
                        NationalNo,
                        FirstName,
                        SecondName,
                        ThirdName,
                        LastName,
                        DateOfBirth,
                        Gendor AS Gender,
                        Address,
                        Phone,
                        Email,
                        NationalityCountryID AS NationalityCountryId,
                        ImagePath
                    FROM People
                    WHERE PersonID = @PersonID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            nationalNo = reader["NationalNo"].ToString();
                            firstName = reader["FirstName"].ToString();
                            secondName = reader["SecondName"].ToString();
                            thirdName = reader["ThirdName"] == DBNull.Value ? null : reader["ThirdName"].ToString();
                            lastName = reader["LastName"].ToString();
                            dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            gender = Convert.ToByte(reader["Gender"]);
                            address = reader["Address"].ToString();
                            phone = reader["Phone"].ToString();
                            email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString();
                            nationalityCountryId = Convert.ToInt32(reader["NationalityCountryId"]);
                            imagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString();
                        }
                    }
                }
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string nationalNo, ref int personId,
            ref string firstName, ref string secondName, ref string thirdName, ref string lastName,
            ref DateTime dateOfBirth, ref byte gender, ref string address, ref string phone,
            ref string email, ref int nationalityCountryId, ref string imagePath)
        {
            bool isFound = false;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    SELECT
                        PersonID AS PersonId,
                        NationalNo,
                        FirstName,
                        SecondName,
                        ThirdName,
                        LastName,
                        DateOfBirth,
                        Gendor AS Gender,
                        Address,
                        Phone,
                        Email,
                        NationalityCountryID AS NationalityCountryId,
                        ImagePath
                    FROM People
                    WHERE NationalNo = @NationalNo;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            personId = Convert.ToInt32(reader["PersonId"]);
                            firstName = reader["FirstName"].ToString();
                            secondName = reader["SecondName"].ToString();
                            thirdName = reader["ThirdName"] == DBNull.Value ? null : reader["ThirdName"].ToString();
                            lastName = reader["LastName"].ToString();
                            dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            gender = Convert.ToByte(reader["Gender"]);
                            address = reader["Address"].ToString();
                            phone = reader["Phone"].ToString();
                            email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString();
                            nationalityCountryId = Convert.ToInt32(reader["NationalityCountryId"]);
                            imagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString();
                        }
                    }
                }
            }

            return isFound;
        }

        public static int AddNewPerson(string nationalNo, string firstName, string secondName, string thirdName,
            string lastName, DateTime dateOfBirth, byte gender, string address, string phone,
            string email, int nationalityCountryId, string imagePath)
        {
            int personId = -1;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    INSERT INTO People 
                    (NationalNo, FirstName, SecondName, ThirdName, LastName, 
                    DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, 
                    ImagePath) 
                    VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, 
                            @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                    SELECT SCOPE_IDENTITY();
                    ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = firstName;
                    command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = secondName;

                    SqlParameter thirdNameParam =
                        command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20);

                    if (string.IsNullOrEmpty(thirdName))
                    {
                        thirdNameParam.Value = DBNull.Value;
                    }
                    else
                    {
                        thirdNameParam.Value = thirdName;
                    }

                    command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = lastName;
                    command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = dateOfBirth;
                    command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = gender;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = address;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone;

                    SqlParameter emailParam =
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, 50);
                    if (string.IsNullOrEmpty(email))
                    {
                        emailParam.Value = DBNull.Value;
                    }
                    else
                    {
                        emailParam.Value = email;
                    }

                    command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = nationalityCountryId;

                    SqlParameter imagePathParam =
                        command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250);
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        imagePathParam.Value = DBNull.Value;
                    }
                    else
                    {
                        imagePathParam.Value = imagePath;
                    }

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedId))
                    {
                        personId = insertedId;
                    }

                }
            }
            return personId;
        }

        public static bool UpdatePerson(int personId, string nationalNo, string firstName, string secondName,
            string thirdName, string lastName, DateTime dateOfBirth, byte gender, string address,
            string phone, string email, int nationalityCountryId, string imagePath)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                UPDATE People
                    SET 
                    NationalNo = @NationalNo, 
                    FirstName = @FirstName,
                    SecondName = @SecondName,
                    ThirdName = @ThirdName,
                    LastName = @LastName,
                    DateOfBirth = @DateOfBirth,
                    Gendor = @Gendor,
                    Address = @Address,
                    Phone = @Phone,
                    Email = @Email,
                    NationalityCountryID = @NationalityCountryID,
                    ImagePath = @ImagePath
                WHERE PersonID = @PersonID;
                ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;
                    command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = firstName;
                    command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = secondName;
                    SqlParameter thirdNameParam =
                        command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20);
                    if (string.IsNullOrEmpty(thirdName))
                    {
                        thirdNameParam.Value = DBNull.Value;
                    }
                    else
                    {
                        thirdNameParam.Value = thirdName;
                    }
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = lastName;
                    command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = dateOfBirth;
                    command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = gender;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = address;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone;

                    SqlParameter emailParam =
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 50);
                    if (string.IsNullOrEmpty(email))
                    {
                        emailParam.Value = DBNull.Value;
                    }
                    else
                    {
                        emailParam.Value = email;
                    }

                    command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = nationalityCountryId;

                    SqlParameter imagePathParam =
                        command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250);
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        imagePathParam.Value = DBNull.Value;
                    }
                    else
                    {
                        imagePathParam.Value = imagePath;
                    }

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected > 0;
        }

        public static bool DeletePerson(int personId)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    DELETE FROM People
                    WHERE PersonID = @PersonID;
                    ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllPersons()
        {
            DataTable dt = new DataTable();

            string query = @"
                            SELECT
                                PersonID AS PersonId,
                                NationalNo,
                                FirstName,
                                SecondName,
                                ThirdName,
                                LastName,
                                DateOfBirth,
                                Gendor AS Gender,
                                Address,
                                Phone,
                                Email,
                                NationalityCountryID AS NationalityCountryId,
                                ImagePath
                            FROM People;
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
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return dt;
        }

        public static bool IsPersonExist(int personId)
        {
            bool isFound = false;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = "SELECT FOUND = 1 FROM People WHERE PersonID = @PersonID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        isFound = (result != null);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return isFound;
        }

        public static bool IsNationalNoExist(string nationalNo)
        {
            bool isFound = false;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = "SELECT FOUND = 1 FROM People WHERE NationalNo = @NationalNo;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = nationalNo;

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        isFound = (result != null);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return isFound;
        }

        public static string GetFullName(int personId)
        {
            string fullName = string.Empty;

            const string query = @"
            SELECT 
                FirstName + ' ' + SecondName
                + CASE 
                    WHEN ThirdName IS NULL OR ThirdName = '' THEN ''
                    ELSE ' ' + ThirdName
                  END
                + ' ' + LastName AS FullName
            FROM People
            WHERE PersonID = @PersonID;
            ";

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personId;

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    fullName = result.ToString();
                }
            }

            return fullName;
        }
    }
}
