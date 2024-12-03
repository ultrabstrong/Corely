namespace Corely.IAM.Users.Exceptions;

public sealed class UserExistsException : Exception
{
    public bool UsernameExists { get; init; }
    public bool EmailExists { get; init; }

    public UserExistsException() : base()
    {
    }

    public UserExistsException(string message) : base(message)
    {
    }

    public UserExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
