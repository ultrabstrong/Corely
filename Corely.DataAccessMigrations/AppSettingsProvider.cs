using Microsoft.Extensions.Configuration;

namespace Corely.DataAccessMigrations
{
    internal static class AppSettingsProvider
    {
        private const string SETTINGS_FILE_NAME = "migrationsettings.json";

        private static readonly IConfigurationRoot _configuration;

        static AppSettingsProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(SETTINGS_FILE_NAME, optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static string GetConnectionString()
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString
                ?? throw new Exception($"DefaultConnection string not found in {SETTINGS_FILE_NAME}");
        }
    }
}
