using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TimeCats
{
    // Based on https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1
    public sealed class CryptographyService
    {
        private const int SALT_SIZE = 256 / 8;
        private const int HASH_SIZE = 256 / 8;
        private const int HASH_ITERATIONS = 10000;
        
        // Generate a 128-bit salt using a secure PRNG
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[SALT_SIZE];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            return salt;
        }

        public string CalculateHash(byte[] salt, string password)
        {
            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: HASH_ITERATIONS,
                numBytesRequested: HASH_SIZE);
            
            string hashed = Convert.ToBase64String(hash);

            return hashed;
        }

        public bool Verify(string hashedPassword, byte[] originalSalt, string password)
        {
            var newHash = CalculateHash(originalSalt, password);
            return hashedPassword.Equals(newHash);
        }
    }
}