using Corely.Security.PasswordValidation.Providers;

namespace Corely.Security.Password;

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
