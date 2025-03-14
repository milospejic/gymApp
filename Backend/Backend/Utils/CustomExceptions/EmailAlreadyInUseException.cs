namespace Backend.Utils.CustomExceptions
{
    /// <summary>
    /// Represents an error that occurs when an attempt is made to register or update an email address that is already in use.
    /// This exception is typically thrown during user registration or profile updates when the email is already associated with an existing account.
    /// </summary>
    public class EmailAlreadyInUseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAlreadyInUseException"/> class with a default error message indicating that the email is already taken.
        /// </summary>
        public EmailAlreadyInUseException() : base("Email already taken") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAlreadyInUseException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EmailAlreadyInUseException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAlreadyInUseException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EmailAlreadyInUseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
