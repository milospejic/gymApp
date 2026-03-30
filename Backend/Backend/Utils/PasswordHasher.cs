using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;

namespace Backend.Utils
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords using BouncyCastle's BCrypt algorithm.
    /// </summary>
    public static class PasswordHasher
    {
        // Cost factor for BCrypt (11 is a strong, modern default balancing security and performance)
        private const int Cost = 11;

        /// <summary>
        /// Hashes the provided password using the OpenBsdBCrypt hashing algorithm.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>A hashed representation of the password.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the password is null or empty.</exception>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
            }

            // 1. Generate a cryptographically secure 16-byte salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // 2. Generate the BCrypt hash. 
            // OpenBsdBCrypt automatically formats this into the standard modular crypt format (e.g., $2a$11$...)
            return OpenBsdBCrypt.Generate(password.ToCharArray(), salt, Cost);
        }

        /// <summary>
        /// Verifies that the provided password matches the stored hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns><c>true</c> if the password matches the hashed password; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Thrown if either the password or hashed password is null or empty.</exception>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentException("Both password and hashed password must be provided.");
            }

            // 3. Verify the hash using BouncyCastle
            return OpenBsdBCrypt.CheckPassword(hashedPassword, password.ToCharArray());
        }
    }
}