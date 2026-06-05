using System;
using Microsoft.Win32;

namespace Presentation.Helpers
{
    public class LoginCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginCredentials()
        {
            UserName = string.Empty;
            Password = string.Empty;
            RememberMe = false;
        }

        public LoginCredentials(string userName, string password, bool rememberMe)
        {
            UserName = userName ?? string.Empty;
            Password = password ?? string.Empty;
            RememberMe = rememberMe;
        }
    }

    public static class CredentialsManager
    {
        private const string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
        private const string userNameValueName = "UserName";
        private const string passwordValueName = "Password";
        private const string rememberMeValueName = "RememberMe";

        public static LoginCredentials Load()
        {
            try
            {
                string userNameVal = Registry.GetValue(keyPath, userNameValueName, null) as string;
                string passwordVal = Registry.GetValue(keyPath, passwordValueName, null) as string;
                bool rememberMeVal = false;
                bool.TryParse(Registry.GetValue(keyPath, rememberMeValueName, null) as string, out rememberMeVal);

                return new LoginCredentials(userNameVal, passwordVal, rememberMeVal);

            }
            catch (Exception ex)
            {
                return new LoginCredentials();
            }
        }

        public static bool Save(string userName, string password, bool rememberMe)
        {
            try
            {
                if (rememberMe)
                {
                    Registry.SetValue(keyPath, userNameValueName, userName, RegistryValueKind.String);
                    Registry.SetValue(keyPath, passwordValueName, password, RegistryValueKind.String);
                    Registry.SetValue(keyPath, rememberMeValueName, rememberMe.ToString(), RegistryValueKind.String);
                }
                else
                {
                    Clear();
                    
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SavePasswordOnly(string password)
        {
            try
            {
                LoginCredentials current = Load();

                // Keep current username + rememberMe, change only password
                return Save(current.UserName, password ?? string.Empty, current.RememberMe);
            }
            catch
            {
                return false;
            }
        }

        public static void Clear()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DVLD", writable: true))
                {
                    key?.DeleteValue(userNameValueName, throwOnMissingValue: false);
                    key?.DeleteValue(passwordValueName, throwOnMissingValue: false);
                    key?.DeleteValue(rememberMeValueName, throwOnMissingValue: false);
                }
            }
            catch
            {
            }
        }
    }
}