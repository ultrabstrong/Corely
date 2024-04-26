namespace Corely.IAM.Exceptions
{
    public class AccountDoesNotExistException : Exception
    {
        public AccountDoesNotExistException() : base()
        {
        }

        public AccountDoesNotExistException(string message) : base(message)
        {
        }

        public AccountDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
