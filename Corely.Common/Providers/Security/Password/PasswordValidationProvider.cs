using System.Text;
using System.Text.RegularExpressions;

namespace Corely.Common.Providers.Security.Password
{
    public class PasswordValidationProvider : IPasswordValidationProvider
    {
        public int MinimumLength { get; init; } = 5;
        public bool RequireUppercase { get; init; } = true;
        public bool RequireLowercase { get; init; } = true;
        public bool RequireNumber { get; init; } = true;
        public bool RequireSpecialCharacter { get; init; } = false;

        private Regex Regex => _regex ??= CreateRegex();
        private Regex? _regex;

        private Regex CreateRegex()
        {
            var patternBuilder = new StringBuilder();

            patternBuilder.Append('^');
            if (RequireUppercase) patternBuilder.Append("(?=.*[A-Z])");
            if (RequireLowercase) patternBuilder.Append("(?=.*[a-z])");
            if (RequireNumber) patternBuilder.Append("(?=.*[0-9])");
            if (RequireSpecialCharacter) patternBuilder.Append("(?=.*[^A-Za-z0-9])");
            patternBuilder.Append($".{{{MinimumLength},}}$");

            return new Regex(patternBuilder.ToString());
        }

        public ValidatePasswordResult[] ValidatePassword(string password)
        {
            if (Regex.IsMatch(password))
            {
                return [ValidatePasswordResult.Success];
            }
            else
            {
                return DetailedValidation(password);
            }
        }

        private ValidatePasswordResult[] DetailedValidation(string password)
        {
            var hasUppercase = !RequireUppercase;
            var hasLowercase = !RequireLowercase;
            var hasDigit = !RequireNumber;
            var hasSpecial = !RequireSpecialCharacter;

            foreach (var ch in password)
            {
                if (!hasUppercase && char.IsUpper(ch)) hasUppercase = true;
                if (!hasLowercase && char.IsLower(ch)) hasLowercase = true;
                if (!hasDigit && char.IsDigit(ch)) hasDigit = true;
                if (!hasSpecial && !char.IsLetterOrDigit(ch)) hasSpecial = true;
            }

            var validationResults = new List<ValidatePasswordResult>();

            if (password.Length < MinimumLength)
            {
                validationResults.Add(ValidatePasswordResult.TooShort);
            }
            if (!hasUppercase)
            {
                validationResults.Add(ValidatePasswordResult.NoUppercase);
            }
            if (!hasLowercase)
            {
                validationResults.Add(ValidatePasswordResult.NoLowercase);
            }
            if (!hasDigit)
            {
                validationResults.Add(ValidatePasswordResult.NoDigit);
            }
            if (!hasSpecial)
            {
                validationResults.Add(ValidatePasswordResult.NoSpecialCharacter);
            }

            return [.. validationResults];
        }
    }
}
