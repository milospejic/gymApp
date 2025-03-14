﻿namespace Backend.Utils.CustomExceptions
{
    /// <summary>
    /// Represents an error that occurs when a bad request is made, typically due to invalid input or parameters.
    /// This exception is often used to indicate client-side errors.
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class with a default error message.
        /// </summary>
        public BadRequestException() : base("Invalid Request") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BadRequestException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
