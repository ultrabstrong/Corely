namespace Corely.Security.Signature;

public sealed class SignatureException : Exception
{
    public enum ErrorReason
    {
        Unknown,
        InvalidFormat,
        InvalidTypeCode
    }

    public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

    public SignatureException() : base()
    {
    }

    public SignatureException(string message) : base(message)
    {
    }

    public SignatureException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
