using Corely.Security.Password;
using Corely.Security.PasswordValidation.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.RegularExpressions;

namespace Corely.Security.PasswordValidation.Providers;

public sealed class PasswordValidationProvider : IPasswordValidationProvider
{
    private readonly PasswordValidationOptions _options;

    public PasswordValidationProvider(IOptions<PasswordValidationOptions> options)
    {
        _options = options.Value;
    }

    private Regex Regex => _regex ??= CreateRegex();
    private Regex? _regex;

    private Regex CreateRegex()
    {
        var patternBuilder = new StringBuilder();

        patternBuilder.Append('^');
        if (_options.RequireUppercase) patternBuilder.Append("(?=.*[A-Z])");
        if (_options.RequireLowercase) patternBuilder.Append("(?=.*[a-z])");
        if (_options.RequireDigit) patternBuilder.Append("(?=.*[0-9])");
        if (_options.RequireNonAlphanumeric) patternBuilder.Append("(?=.*[^A-Za-z0-9])");
        patternBuilder.Append($".{{{_options.MinimumLength},}}$");

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
        var hasUppercase = !_options.RequireUppercase;
        var hasLowercase = !_options.RequireLowercase;
        var hasDigit = !_options.RequireDigit;
        var hasSpecial = !_options.RequireNonAlphanumeric;

        foreach (var c in password)
        {
            if (!hasUppercase && char.IsUpper(c)) hasUppercase = true;
            if (!hasLowercase && char.IsLower(c)) hasLowercase = true;
            if (!hasDigit && char.IsDigit(c)) hasDigit = true;
            if (!hasSpecial && !char.IsLetterOrDigit(c)) hasSpecial = true;
        }

        var validationResults = new List<string>();

        if (password.Length < _options.MinimumLength)
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
