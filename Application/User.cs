using Business.Common;
using Business.Security;
using Contracts.DTOs;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class User
    {
        public EntityMode Mode { get; private set; }

        public int UserId { get; private set; }
        public int PersonId { get; private set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; } 
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }

        public User()
        {
            UserId = -1;
            PersonId = -1;
            UserName = "";
            IsActive = false;
            PasswordHash = "";
            PasswordSalt = "";

            Mode = EntityMode.AddNew;
        }

        private User(UserDto user)
        {
            UserId = user.UserId;
            PersonId = user.PersonId;
            UserName = user.UserName;
            IsActive = user.IsActive;
            PasswordHash = user.PasswordHash;
            PasswordSalt = user.PasswordSalt;

            Mode = EntityMode.Update;
        }

        private UserDto ToDto()
        {
            return new UserDto
            {
                UserId = UserId,
                PersonId = PersonId,
                UserName = UserName,
                IsActive = IsActive,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt
            };
        }
        public void SetPassword(string password)
        {
            var (hash, salt) = PasswordHasher.HashPassword(password);
            PasswordHash = hash;
            PasswordSalt = salt; 
        }

        public bool VerifyPassword(string password)
        {
            return PasswordHasher.VerifyPassword(password, PasswordHash, PasswordSalt);
        }

        public bool ChangePassword(string newPassword, string oldPassword)
        {
            if (Mode == EntityMode.AddNew || !VerifyPassword(oldPassword))
            {
                return false;
            }

            var (hash, salt) = PasswordHasher.HashPassword(newPassword);

            if (UserData.UpdateUserPassword(UserId, hash, salt))
            {
                PasswordHash = hash;
                PasswordSalt = salt;
                return true;
            }
            return false;
        }

        // Resets user password without requiring old password verification.
        // Should only be called by administrators for password recovery scenarios.
        public bool ResetPassword(string newPassword)
        {
            // Prevent resetting password for unsaved users
            if (Mode == EntityMode.AddNew)
            {
                return false;
            }

            var (hash, salt) = PasswordHasher.HashPassword(newPassword);

            if (UserData.UpdateUserPassword(UserId, hash, salt))
            {
                PasswordHash = hash;
                PasswordSalt = salt;
                return true;
            }
            return false;
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

        private bool AddNew()
        {
            UserId = UserData.AddNewUser(ToDto());
            return UserId != -1;
        }

        private bool Update()
        {
            return UserData.UpdateUser(ToDto());
        }

        public static bool Delete(int userId)
        {
            return UserData.DeleteUser(userId);
        }

        public static User Find(int userId)
        {
            UserDto dto = UserData.GetUserByUserId(userId);
            return dto == null ? null : new User(dto);
        }

        public static List<UserDto> GetAllUsers()
        {
            return UserData.GetAllUsers();
        }

        public static bool IsUserExistByPersonId(int personId)
        {
            return UserData.IsUserExistsByPersonId(personId);
        }
    }
}
