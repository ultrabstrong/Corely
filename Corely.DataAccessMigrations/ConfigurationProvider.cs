﻿using Microsoft.Extensions.Configuration;

namespace Corely.DataAccessMigrations
{
    internal static class ConfigurationProvider
    {
        private const string SETTINGS_FILE_NAME = "migrationsettings.json";

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
            return _configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception($"DefaultConnection string not found in {SETTINGS_FILE_NAME}");
        }
    }
}
