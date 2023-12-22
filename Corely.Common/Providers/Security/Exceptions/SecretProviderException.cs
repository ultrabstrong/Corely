namespace Corely.Common.Providers.Security.Exceptions
{
    public sealed class SecretProviderException : Exception
    {
        public enum ErrorReason
        {
            Unknown,
            InvalidVersion,
            CurrentKeyNotFound,
        }

        public ErrorReason Reason { get; set; } = ErrorReason.Unknown;

        public SecretProviderException() : base()
        {
        }

        public SecretProviderException(string message) : base(message)
        {
        }

        public SecretProviderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
