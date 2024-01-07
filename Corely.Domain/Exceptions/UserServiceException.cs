namespace Corely.Domain.Exceptions
{
    public sealed class UserServiceException : Exception
    {
        public enum ErrorReason
        {
            Unknown,
            UserAlreadyExists,
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

        public UserServiceException() : base()
        {
        }

        public UserServiceException(string message) : base(message)
        {
        }

        public UserServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
