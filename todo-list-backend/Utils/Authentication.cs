using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace todo_list_backend.Utils
{
    public static class Authentication
    {
        private static byte[] Hash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);
        }

        public static Tuple<string, string> GenerateHashAndSalt(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = Hash(password, salt);

            return new Tuple<string, string>(
                Convert.ToBase64String(hash),
                Convert.ToBase64String(salt)
            );
        }

        public static bool VerifyPassword(string hash, string salt, string password)
        {
            string test = Convert.ToBase64String(
                Hash(password, Convert.FromBase64String(salt))
            );

            return hash == test;
        }
    }
}
