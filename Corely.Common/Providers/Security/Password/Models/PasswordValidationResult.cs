namespace Corely.Common.Providers.Security.Password.Models
{
    public record PasswordValidationResult(
        bool IsSuccess,
        string[] ValidationFailures)
    {
    }
}
