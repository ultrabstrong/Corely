using Corely.IAM.Validators;

namespace Corely.IAM.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public ValidationResult? ValidationResult { get; init; }

        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
