namespace Corely.IAM.Users.Exceptions;

public class UserDoesNotExistException : Exception
{
    public UserDoesNotExistException() : base()
    {
    }

    public UserDoesNotExistException(string message) : base(message)
    {
    }

    public UserDoesNotExistException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
