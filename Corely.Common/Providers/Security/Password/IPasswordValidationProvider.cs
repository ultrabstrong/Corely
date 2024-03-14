namespace Corely.Common.Providers.Security.Password
{
    public interface IPasswordValidationProvider
    {
        int MinimumLength { get; }
        bool RequireUppercase { get; }
        bool RequireLowercase { get; }
        bool RequireNumber { get; }
        bool RequireSpecialCharacter { get; }

        ValidatePasswordResult[] ValidatePassword(string password);
    }
}
