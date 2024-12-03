namespace Corely.IAM.Groups.Exceptions;

public class GroupDoesNotExistException : Exception
{
    public GroupDoesNotExistException() : base()
    {
    }

    public GroupDoesNotExistException(string message) : base(message)
    {
    }

    public GroupDoesNotExistException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
