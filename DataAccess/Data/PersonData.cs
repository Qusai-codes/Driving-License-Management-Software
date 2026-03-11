using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.DTOs;
using DataAccess.Common;

namespace DataAccess.Data
{

    public class PersonData
    {
        public static PersonDto GetPersonInfoById(int id)
        {
            PersonDto person = null;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    SELECT
                        PersonID,
                        NationalNo,
                        FirstName,
                        SecondName,
                        ThirdName,
                        LastName,
                        DateOfBirth,
                        Gendor,
                        Address,
                        Phone,
                        Email,
                        NationalityCountryID,
                        ImagePath
                    FROM People
                    WHERE PersonID = @PersonID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = id;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person = new PersonDto
                            {
                                PersonId = Convert.ToInt32(reader["PersonID"]),
                                NationalNo = reader["NationalNo"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                SecondName = reader["SecondName"].ToString(),

                                ThirdName = reader["ThirdName"] == DBNull.Value
                                    ? null
                                    : reader["ThirdName"].ToString(),

                                LastName = reader["LastName"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Gender = Convert.ToByte(reader["Gendor"]),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),

                                Email = reader["Email"] == DBNull.Value
                                    ? null
                                    : reader["Email"].ToString(),

                                NationalityCountryId = Convert.ToInt32(reader["NationalityCountryID"]),

                                ImagePath = reader["ImagePath"] == DBNull.Value
                                    ? null
                                    : reader["ImagePath"].ToString()
                            };
                        }
                    }
                }
            }

            return person;
        }

        public static PersonDto GetPersonInfoByNationalNo(string nationalNo)
        {
            PersonDto person = null;

            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                string query = @"
                    SELECT
                        PersonID,
                        NationalNo,
                        FirstName,
                        SecondName,
                        ThirdName,
                        LastName,
                        DateOfBirth,
                        Gendor,
                        Address,
                        Phone,
                        Email,
                        NationalityCountryID,
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
                            person = new PersonDto
                            {
                                PersonId = Convert.ToInt32(reader["PersonID"]),
                                NationalNo = reader["NationalNo"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                SecondName = reader["SecondName"].ToString(),

                                ThirdName = reader["ThirdName"] == DBNull.Value
                                    ? null
                                    : reader["ThirdName"].ToString(),

                                LastName = reader["LastName"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Gender = Convert.ToByte(reader["Gendor"]),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),

                                Email = reader["Email"] == DBNull.Value
                                    ? null
                                    : reader["Email"].ToString(),

                                NationalityCountryId = Convert.ToInt32(reader["NationalityCountryID"]),

                                ImagePath = reader["ImagePath"] == DBNull.Value
                                    ? null
                                    : reader["ImagePath"].ToString()
                            };
                        }
                    }
                }
            }

            return person;
        }

        public static int AddNewPerson(PersonDto person)
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
                    command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = person.NationalNo;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = person.FirstName;
                    command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = person.SecondName;

                    SqlParameter thirdNameParam =
                        command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20);

                    if (string.IsNullOrEmpty(person.ThirdName))
                    {
                        thirdNameParam.Value = DBNull.Value;
                    }
                    else
                    {
                        thirdNameParam.Value = person.ThirdName;
                    }

                    command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = person.LastName;
                    command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = person.DateOfBirth;
                    command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = person.Gender;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = person.Address;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;

                    SqlParameter emailParam =
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, 50);
                    if (string.IsNullOrEmpty(person.Email))
                    {
                        emailParam.Value = DBNull.Value;
                    }
                    else
                    {
                        emailParam.Value = person.Email;
                    }

                    command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = person.NationalityCountryId;

                    SqlParameter imagePathParam =
                        command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250);
                    if (string.IsNullOrEmpty(person.ImagePath))
                    {
                        imagePathParam.Value = DBNull.Value;
                    }
                    else
                    {
                        imagePathParam.Value = person.ImagePath;
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

        public static bool UpdatePerson(PersonDto person)
        {
            int rowsAffected = 0;
            if (person == null) throw new ArgumentNullException(nameof(person));

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
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = person.PersonId;
                    command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = person.NationalNo;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = person.FirstName;
                    command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = person.SecondName;
                    SqlParameter thirdNameParam =
                        command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20);
                    if (string.IsNullOrEmpty(person.ThirdName))
                    {
                        thirdNameParam.Value = DBNull.Value;
                    }
                    else
                    {
                        thirdNameParam.Value = person.ThirdName;
                    }
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = person.LastName;
                    command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = person.DateOfBirth;
                    command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = person.Gender;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = person.Address;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;

                    SqlParameter emailParam =
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 50);
                    if (string.IsNullOrEmpty(person.Email))
                    {
                        emailParam.Value = DBNull.Value;
                    }
                    else
                    {
                        emailParam.Value = person.Email;
                    }

                    command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = person.NationalityCountryId;

                    SqlParameter imagePathParam =
                        command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250);
                    if (string.IsNullOrEmpty(person.ImagePath))
                    {
                        imagePathParam.Value = DBNull.Value;
                    }
                    else
                    {
                        imagePathParam.Value = person.ImagePath;
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

        public static List<PersonDto> GetAllPersons()
        {
            List<PersonDto> people = new List<PersonDto>();

            string query = @"
                            SELECT
                                PersonID,
                                NationalNo,
                                FirstName,
                                SecondName,
                                ThirdName,
                                LastName,
                                DateOfBirth,
                                Gendor,
                                Address,
                                Phone,
                                Email,
                                NationalityCountryID,
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
                            while (reader.Read())
                            {
                                people.Add(new PersonDto
                                {
                                    PersonId = Convert.ToInt32(reader["PersonID"]),
                                    NationalNo = reader["NationalNo"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    SecondName = reader["SecondName"].ToString(),

                                    ThirdName = reader["ThirdName"] == DBNull.Value
                                        ? null
                                        : reader["ThirdName"].ToString(),

                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                    Gender = Convert.ToByte(reader["Gendor"]),
                                    Address = reader["Address"].ToString(),
                                    Phone = reader["Phone"].ToString(),

                                    Email = reader["Email"] == DBNull.Value
                                        ? null
                                        : reader["Email"].ToString(),

                                    NationalityCountryId = Convert.ToInt32(reader["NationalityCountryID"]),

                                    ImagePath = reader["ImagePath"] == DBNull.Value
                                        ? null
                                        : reader["ImagePath"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return people;
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
    }
}
