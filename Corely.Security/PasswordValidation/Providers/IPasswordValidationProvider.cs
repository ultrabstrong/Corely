namespace Corely.Security.PasswordValidation.Providers
{
    public interface IPasswordValidationProvider
    {
        int MinimumLength { get; }
        bool RequireUppercase { get; }
        bool RequireLowercase { get; }
        bool RequireNumber { get; }
        bool RequireSpecialCharacter { get; }

        PasswordValidationResult ValidatePassword(string password);
    }
}
