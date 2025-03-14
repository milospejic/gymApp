namespace Backend.Utils
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords using the BCrypt hashing algorithm.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes the provided password using the BCrypt hashing algorithm.
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

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifies that the provided password matches the stored hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns><c>true</c> if the password matches the hashed password; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the password or hashed password is null or empty.</exception>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentNullException("Both password and hashed password must be provided.");
            }

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

