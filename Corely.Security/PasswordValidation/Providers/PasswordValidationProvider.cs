using Corely.Security.Password;
using System.Text;
using System.Text.RegularExpressions;

namespace Corely.Security.PasswordValidation.Providers
{
    public sealed class PasswordValidationProvider : IPasswordValidationProvider
    {
        public int MinimumLength { get; init; } = 5;
        public bool RequireUppercase { get; init; } = true;
        public bool RequireLowercase { get; init; } = true;
        public bool RequireDigit { get; init; } = true;
        public bool RequireNonAlphanumeric { get; init; } = false;

        private Regex Regex => _regex ??= CreateRegex();
        private Regex? _regex;

        private Regex CreateRegex()
        {
            var patternBuilder = new StringBuilder();

            patternBuilder.Append('^');
            if (RequireUppercase) patternBuilder.Append("(?=.*[A-Z])");
            if (RequireLowercase) patternBuilder.Append("(?=.*[a-z])");
            if (RequireDigit) patternBuilder.Append("(?=.*[0-9])");
            if (RequireNonAlphanumeric) patternBuilder.Append("(?=.*[^A-Za-z0-9])");
            patternBuilder.Append($".{{{MinimumLength},}}$");

            return new Regex(patternBuilder.ToString());
        }

        public PasswordValidationResult ValidatePassword(string password)
        {
            if (Regex.IsMatch(password))
            {
                return new(true, []);
            }
            else
            {
                return DetailedValidation(password);
            }
        }

        private PasswordValidationResult DetailedValidation(string password)
        {
            var hasUppercase = !RequireUppercase;
            var hasLowercase = !RequireLowercase;
            var hasDigit = !RequireDigit;
            var hasSpecial = !RequireNonAlphanumeric;

            foreach (var c in password)
            {
                if (!hasUppercase && char.IsUpper(c)) hasUppercase = true;
                if (!hasLowercase && char.IsLower(c)) hasLowercase = true;
                if (!hasDigit && char.IsDigit(c)) hasDigit = true;
                if (!hasSpecial && !char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            var validationResults = new List<string>();

            if (password.Length < MinimumLength)
            {
                validationResults.Add(PasswordValidationConstants.PASSWORD_TOO_SHORT);
            }
            if (!hasUppercase)
            {
                validationResults.Add(PasswordValidationConstants.PASSWORD_MISSING_UPPERCASE);
            }
            if (!hasLowercase)
            {
                validationResults.Add(PasswordValidationConstants.PASSWORD_MISSING_LOWERCASE);
            }
            if (!hasDigit)
            {
                validationResults.Add(PasswordValidationConstants.PASSWORD_MISSING_DIGIT);
            }
            if (!hasSpecial)
            {
                validationResults.Add(PasswordValidationConstants.PASSWORD_MISSING_SPECIAL_CHARACTER);
            }

            return new(false, [.. validationResults]);
        }
    }
}
