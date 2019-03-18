using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;

namespace VdarApi.Helpers
{
    public sealed class SecurePasswordHasherHelper
    {
        
        public static bool Validate(string password, string salt, string hash)
            => Hash(password, salt) == hash;

        public static string Hash(string password, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static string GenerateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
