using Corely.IAM.Security.Models;
using Corely.Security.PasswordValidation.Providers;
using Microsoft.Extensions.Configuration;

namespace ConsoleTest;

internal static class ConfigurationProvider
{
    private const string SETTINGS_FILE_NAME = "appsettings.json";

    private static readonly IConfigurationRoot _configuration;

    static ConfigurationProvider()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(SETTINGS_FILE_NAME, optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }

    public static string GetConnectionString() =>
        _configuration.GetConnectionString("DefaultConnection")
        ?? throw new Exception($"DefaultConnection string not found in {SETTINGS_FILE_NAME}");

    public static string GetSystemSymmetricEncryptionKey() =>
        _configuration["SystemSymmetricEncryptionKey"]
        ?? throw new Exception($"SystemSymmetricEncryptionKey not found in {SETTINGS_FILE_NAME}");

    public static PasswordValidationProvider GetPasswordValidationProvider() =>
        _configuration.GetSection("PasswordValidation").Get<PasswordValidationProvider>()
        ?? throw new Exception($"PasswordValidation section not found in {SETTINGS_FILE_NAME}");

    public static SecurityOptions GetSecurityOptions() =>
        _configuration.GetSection("SecurityOptions").Get<SecurityOptions>()
        ?? throw new Exception($"SecurityOptions section not found in {SETTINGS_FILE_NAME}");
}