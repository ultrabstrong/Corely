namespace Corely.Security.KeyStore
{
    public sealed class KeyStoreException : Exception
    {
        public enum ErrorReason
        {
            Unknown,
            InvalidVersion,
            CurrentKeyNotFound,
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

        public KeyStoreException() : base()
        {
        }

        public KeyStoreException(string message) : base(message)
        {
        }

        public KeyStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
