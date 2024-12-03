namespace Corely.Security.Hashing;

public class HashException : Exception
{
    public enum ErrorReason
    {
        Unknown,
        InvalidFormat,
        InvalidTypeCode
    }

    public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

    public HashException() : base()
    {
    }

    public HashException(string message) : base(message)
    {
    }

    public HashException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
