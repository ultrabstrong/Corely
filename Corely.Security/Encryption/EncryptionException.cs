namespace Corely.Security.Encryption
{
    public sealed class EncryptionException : Exception
    {
        public enum ErrorReason
        {
            Unknown,
            InvalidFormat,
            InvalidTypeCode
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

        public EncryptionException() : base()
        {
        }

        public EncryptionException(string message) : base(message)
        {
        }

        public EncryptionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
