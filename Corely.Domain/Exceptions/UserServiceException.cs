namespace Corely.Domain.Exceptions
{
    public class UserServiceException : Exception
    {
        public enum ErrorReason
        {
            Undefined,
            UserAlreadyExists
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Undefined;

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
