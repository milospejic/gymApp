namespace Backend.Utils.CustomExceptions
{
    public class EmailAlreadyInUseException : Exception
    {
        public EmailAlreadyInUseException() : base("Email already taken") { }

        public EmailAlreadyInUseException(string message) : base(message) { }

        public EmailAlreadyInUseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
