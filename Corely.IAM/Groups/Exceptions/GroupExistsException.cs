namespace Corely.IAM.Groups.Exceptions;

public class GroupExistsException : Exception
{
    public GroupExistsException() : base()
    {
    }
    public GroupExistsException(string message) : base(message)
    {
    }
    public GroupExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
