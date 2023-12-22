namespace Corely.Common.Providers.Security.Exceptions
{
    public sealed class KeyStoreProviderException : Exception
    {
        public enum ErrorReason
        {
            Unknown,
            InvalidVersion,
            CurrentKeyNotFound,
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

        public KeyStoreProviderException() : base()
        {
        }

        public KeyStoreProviderException(string message) : base(message)
        {
        }

        public KeyStoreProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
