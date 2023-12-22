namespace Corely.Common.Providers.Security.Exceptions
{
    public class HashProviderException : Exception
    {
        public enum ErrorReason
        {
            Unknown,
            InvalidFormat,
            InvalidTypeCode
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

        public HashProviderException() : base()
        {
        }

        public HashProviderException(string message) : base(message)
        {
        }

        public HashProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
