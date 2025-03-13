namespace Backend.Utils.CustomExceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Invalid Request") { }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
