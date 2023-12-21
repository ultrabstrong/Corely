using Corely.Domain.Validators;

namespace Corely.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationResult ValidationResult { get; init; }

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
