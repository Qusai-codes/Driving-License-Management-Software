using System;
using System.Configuration;
using System.IO;

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
        private static string CredentialsFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["LoginCredentialsFilePath"];
            }
        }

        public static LoginCredentials Load()
        {
            try
            {
                if (!File.Exists(CredentialsFilePath))
                {
                    return new LoginCredentials();
                }

                string[] lines = File.ReadAllLines(CredentialsFilePath);

                if (lines.Length >= 3)
                {
                    string userName = lines[0];
                    string password = lines[1];
                    bool rememberMe = bool.TryParse(lines[2], out bool result) && result;

                    return new LoginCredentials(userName, password, rememberMe);
                }

                return new LoginCredentials();
            }
            catch
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
                    string[] credentials = new string[]
                    {
                        userName ?? string.Empty,
                        password ?? string.Empty,
                        rememberMe.ToString()
                    };

                    // Create directory if it doesn't exist
                    string directory = Path.GetDirectoryName(CredentialsFilePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Write credentials to file (creates file if it doesn't exist)
                    File.WriteAllLines(CredentialsFilePath, credentials);
                    return true;
                }
                else
                {
                    // Don't save credentials if RememberMe is false - delete file instead
                    Clear();
                    return true;
                }
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
                if (File.Exists(CredentialsFilePath))
                {
                    File.Delete(CredentialsFilePath);
                }
            }
            catch
            {
            }
        }
    }
}