namespace Corely.Common.Providers.Security.Exceptions
{
    public sealed class EncryptionProviderException : Exception
    {
        public enum ErrorReason
        {
            Unknown,
            InvalidFormat,
            InvalidTypeCode
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

        public EncryptionProviderException() : base()
        {
        }

        public EncryptionProviderException(string message) : base(message)
        {
        }

        public EncryptionProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
