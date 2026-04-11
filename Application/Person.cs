using Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess.Data;
using Business.Common;

namespace Business
{
    public class Person
    {
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
        private Person(PersonDto dto)
        {
            PersonId = dto.PersonId;
            NationalNo = dto.NationalNo;
            FirstName = dto.FirstName;
            SecondName = dto.SecondName;
            ThirdName = dto.ThirdName;
            LastName = dto.LastName;
            DateOfBirth = dto.DateOfBirth;
            Gender = dto.Gender;
            Address = dto.Address;
            Phone = dto.Phone;
            Email = dto.Email;
            NationalityCountryID = dto.NationalityCountryId;
            ImagePath = dto.ImagePath;

            Mode = EntityMode.Update;
        }

        private PersonDto ToDto()
        {
            return new PersonDto
            {
                PersonId = PersonId,
                NationalNo = NationalNo,
                FirstName = FirstName,
                SecondName = SecondName,
                ThirdName = ThirdName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Address = Address,
                Phone = Phone,
                Email = Email,
                NationalityCountryId = NationalityCountryID,
                ImagePath = ImagePath
            };
        }

        private bool AddNew()
        {
            PersonId = PersonData.AddNewPerson(ToDto());
            return PersonId != -1;
        }

        private bool Update()
        {
            return PersonData.UpdatePerson(ToDto());
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

        public static List<PersonDto> GetAllPersons()
        {
            return PersonData.GetAllPersons();
        }

        public static Person Find(int personId)
        {
            PersonDto dto = PersonData.GetPersonInfoById(personId);
            return dto == null ? null : new Person(dto);
        }

        public static Person Find(string nationalNo)
        {
            PersonDto dto = PersonData.GetPersonInfoByNationalNo(nationalNo);
            return dto == null ? null : new Person(dto);
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
