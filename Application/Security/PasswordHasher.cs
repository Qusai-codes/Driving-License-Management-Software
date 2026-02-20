using System;
using System.Security.Cryptography;
using System.Text;


namespace Business.Security
{
    public static class PasswordHasher
    {
        private const int _saltSize = 16;       // 128-bit
        private const int _keySize = 32;        // 256-bit
        private const int _iterations = 10000;

        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Generate random salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[_saltSize];
                rng.GetBytes(saltBytes);

                var hashBytes = GetPbkdf2Bytes(password, saltBytes, _iterations, _keySize);

                var hash = Convert.ToBase64String(hashBytes);
                var salt = Convert.ToBase64String(saltBytes);

                return (hash, salt);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var hashBytes = GetPbkdf2Bytes(password, saltBytes, _iterations, _keySize);
            var computedHash = Convert.ToBase64String(hashBytes);

            return SlowEquals(storedHash, computedHash);
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        private static bool SlowEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            var diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}
