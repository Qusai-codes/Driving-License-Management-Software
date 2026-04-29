using Business.Common;
using Business.Security;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
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
        public int PersonId { get; set; }
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

        private User(int userId, int personId, string userName, bool isActive,
            string passwordHash, string passwordSalt)
        {
            UserId = userId;
            PersonId = personId;
            UserName = userName;
            IsActive = isActive;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;

            Mode = EntityMode.Update;
        }

        public void SetPassword(string password)
        {
            var (hash, salt) = PasswordHasher.HashPassword(password);
            PasswordHash = hash;
            PasswordSalt = salt;
        }

        public static bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            return PasswordHasher.VerifyPassword(password, passwordHash, passwordSalt);
        }

        public static bool CheckPassword(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
                return false;

            var passwordData = GetPasswordData(userName);

            if (string.IsNullOrWhiteSpace(passwordData.Hash) ||
                string.IsNullOrWhiteSpace(passwordData.Salt))
            {
                return false;
            }

            return VerifyPassword(password, passwordData.Hash, passwordData.Salt);
        }

        public bool ChangePassword(string newPassword, string oldPassword)
        {
            if (Mode == EntityMode.AddNew || !VerifyPassword(oldPassword, PasswordHash, PasswordSalt))
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

        public static (string Hash, string Salt) GetPasswordData(string userName)
        {
            return UserData.GetPasswordData(userName);
        }

        public static bool IsUserActive(string userName)
        {
            return UserData.IsUserActive(userName);
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
            UserId = UserData.AddNewUser(PersonId, UserName, PasswordHash, PasswordSalt, IsActive);
            return UserId != -1;
        }

        private bool Update()
        {
            return UserData.UpdateUser(UserId, PersonId, UserName, IsActive);
        }

        public static bool CanDeleteUser(int userId)
        {
            // business logic of deleting user
            // TODO: Complete the function.
            return Delete(userId);
        }

        public static bool Delete(int userId)
        {
            return UserData.DeleteUser(userId);
        }

        public static User Find(int userId)
        {
            int personId = -1;
            string userName = "", passwordHash = "", passwordSalt = "";
            bool isActive = false;

            if (UserData.GetUserByUserId(userId, ref personId, ref userName, ref isActive,
                ref passwordHash, ref passwordSalt))
            {
                return new User(userId, personId, userName, isActive, passwordHash, passwordSalt);
            }

            return null;
        }

        public static User Find(string userName)
        {
            int userId = -1, personId = -1;
            string dbUserName = "", passwordHash = "", passwordSalt = "";
            bool isActive = false;

            if (UserData.GetUserByUserName(userName, ref userId, ref personId, ref dbUserName,
                ref isActive, ref passwordHash, ref passwordSalt))
            {
                return new User(userId, personId, dbUserName, isActive, passwordHash, passwordSalt);
            }

            return null;
        }

        public static bool HasUsers()
        {
            return UserData.HasUsers();
        }

        public static DataTable GetAllUsers()
        {
            return UserData.GetAllUsers();
        }

        public static bool IsUserExistByPersonId(int personId)
        {
            return UserData.IsUserExistsByPersonId(personId);
        }

        public static bool IsUserExistByUserName(string userName)
        {
            return UserData.IsUserExistByUserName(userName);
        }

        public static int GetPersonId(int userId)
        {
            return UserData.GetPersonId(userId);
        }
    }
}
