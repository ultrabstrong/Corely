using Microsoft.Extensions.Configuration;

namespace ConsoleTest
{
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

        public static string GetConnectionString()
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            return connectionString
                ?? throw new Exception($"DefaultConnection string not found in {SETTINGS_FILE_NAME}");
        }

        public static string GetSystemKey()
        {
            string? systemKey = _configuration["SystemKey"];

            return systemKey
                ?? throw new Exception($"SystemKey not found in {SETTINGS_FILE_NAME}");
        }
    }
}
