namespace Corely.Security.PasswordValidation.Providers;

public interface IPasswordValidationProvider
{
    PasswordValidationResult ValidatePassword(string password);
}
