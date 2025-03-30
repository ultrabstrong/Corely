namespace Corely.Security.PasswordValidation.Models;

public record PasswordValidationOptions
{
    public const string NAME = "PasswordValidationOptions";
    public int MinimumLength { get; init; } = 5;
    public bool RequireUppercase { get; init; } = true;
    public bool RequireLowercase { get; init; } = true;
    public bool RequireDigit { get; init; } = true;
    public bool RequireNonAlphanumeric { get; init; } = false;
}
