namespace Backend.Utils.CustomExceptions
{
    /// <summary>
    /// Represents an error that occurs when a requested resource is not found.
    /// This exception is typically thrown when a resource (such as an entity or record) does not exist in the system,
    /// and the operation cannot proceed without it.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a default error message 
        /// indicating that the requested resource was not found.
        /// </summary>
        public NotFoundException() : base("The requested resource was not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
