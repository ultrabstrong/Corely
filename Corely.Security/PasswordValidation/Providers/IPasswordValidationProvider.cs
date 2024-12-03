namespace Corely.Security.PasswordValidation.Providers;

public interface IPasswordValidationProvider
{
    int MinimumLength { get; }
    bool RequireUppercase { get; }
    bool RequireLowercase { get; }
    bool RequireDigit { get; }
    bool RequireNonAlphanumeric { get; }

    PasswordValidationResult ValidatePassword(string password);
}
