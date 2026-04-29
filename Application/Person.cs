using Business.Common;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Person
    {
        public enum PersonGender : byte
        {
            Male = 0,
            Female = 1
        }

        public EntityMode Mode { get; private set; }

        public int PersonId { get; private set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        // For new Person
        public Person()
        {
            PersonId = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gender = 0;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            ImagePath = "";

            Mode = EntityMode.AddNew;
        }

        // For existing Person (loaded from DB)
        private Person(int personId, string nationalNo, string firstName, string secondName,
            string thirdName, string lastName, DateTime dateOfBirth, byte gender,
            string address, string phone, string email, int nationalityCountryId, string imagePath)
        {
            PersonId = personId;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            NationalityCountryID = nationalityCountryId;
            ImagePath = imagePath;

            Mode = EntityMode.Update;
        }

        private bool AddNew()
        {
            PersonId = PersonData.AddNewPerson(
                NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                Gender, Address, Phone, Email, NationalityCountryID, ImagePath);

            return PersonId != -1;
        }

        private bool Update()
        {
            return PersonData.UpdatePerson(
                PersonId, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth,
                Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case EntityMode.AddNew:
                    if (AddNew())
                    {
                        Mode = EntityMode.Update;
                        return true;
                    }
                    return false;

                case EntityMode.Update:
                    return Update();
            }

            return false;
        }

        public static DataTable GetAllPersons()
        {
            return PersonData.GetAllPersons();
        }

        public static Person Find(int personId)
        {
            string nationalNo = "", firstName = "", secondName = "", thirdName = null,
                lastName = "", address = "", phone = "", email = null, imagePath = null;
            DateTime dateOfBirth = DateTime.Now;
            byte gender = 0;
            int nationalityCountryId = -1;

            if (PersonData.GetPersonInfoById(personId, ref nationalNo, ref firstName, ref secondName,
                ref thirdName, ref lastName, ref dateOfBirth, ref gender, ref address, ref phone,
                ref email, ref nationalityCountryId, ref imagePath))
            {
                return new Person(personId, nationalNo, firstName, secondName, thirdName, lastName,
                    dateOfBirth, gender, address, phone, email, nationalityCountryId, imagePath);
            }

            return null;
        }

        public static Person Find(string nationalNo)
        {
            int personId = -1;
            string firstName = "", secondName = "", thirdName = null, lastName = "",
                address = "", phone = "", email = null, imagePath = null;
            DateTime dateOfBirth = DateTime.Now;
            byte gender = 0;
            int nationalityCountryId = -1;

            if (PersonData.GetPersonInfoByNationalNo(nationalNo, ref personId, ref firstName,
                ref secondName, ref thirdName, ref lastName, ref dateOfBirth, ref gender,
                ref address, ref phone, ref email, ref nationalityCountryId, ref imagePath))
            {
                return new Person(personId, nationalNo, firstName, secondName, thirdName, lastName,
                    dateOfBirth, gender, address, phone, email, nationalityCountryId, imagePath);
            }

            return null;
        }

        public static bool Delete(int personId)
        {
            return PersonData.DeletePerson(personId);
        }

        public static bool Exists(int personId)
        {
            return PersonData.IsPersonExist(personId);
        }

        public static bool IsNationalNoExists(string nationalNo)
        {
            return PersonData.IsNationalNoExist(nationalNo);
        }

        public static string GetFullName(int personId)
        {
            return PersonData.GetFullName(personId);
        }

        public static int CalculatePersonAge(int personId)
        {
            Person person = Find(personId);
            if (person == null)
            {
                return -1;
            }

            DateTime today = DateTime.Today;
            DateTime birthDate = person.DateOfBirth.Date;

            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
