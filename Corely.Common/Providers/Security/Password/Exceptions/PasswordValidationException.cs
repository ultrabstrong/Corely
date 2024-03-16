using Corely.Common.Providers.Security.Password.Models;

namespace Corely.Common.Providers.Security.Password.Exceptions
{
    public class PasswordValidationException : Exception
    {
        public PasswordValidationException(
            PasswordValidationResult validationResult)
            : this(validationResult, default, default)
        {
        }

        public PasswordValidationException(
            PasswordValidationResult validationResult,
            string message)
            : this(validationResult, message, default)
        {
        }

        public PasswordValidationException(
            PasswordValidationResult validationResult,
            string message,
            Exception inner)
            : base(message, inner)
        {
            PasswordValidationResult = validationResult;
        }

        public PasswordValidationResult PasswordValidationResult { get; private init; }
    }
}
