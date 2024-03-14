namespace Corely.Common.Providers.Security.Password
{
    public enum ValidatePasswordResult
    {
        Success,
        TooShort,
        NoUppercase,
        NoLowercase,
        NoDigit,
        NoSpecialCharacter
    }
}
