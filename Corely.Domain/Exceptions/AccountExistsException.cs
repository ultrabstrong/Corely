namespace Corely.Domain.Exceptions
{
    public class AccountExistsException : Exception
    {
        public AccountExistsException() : base()
        {
        }

        public AccountExistsException(string message) : base(message)
        {
        }

        public AccountExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
