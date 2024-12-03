namespace Corely.Security.PasswordValidation.Providers;

public record PasswordValidationResult(
    bool IsSuccess,
    string[] ValidationFailures)
{
}
