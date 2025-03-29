using ConsoleTest.SerilogCustomization;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace ConsoleTest;

internal class ServiceFactory(IConfiguration configuration) : EFServiceFactory
{
    private readonly IConfiguration _configuration = configuration;

    protected override void AddLogging(ILoggingBuilder builder)
    {
        builder.AddSerilog(logger: Log.Logger, dispose: false);
    }

    protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
        => new SecurityConfigurationProvider(
            _configuration["SystemSymmetricEncryptionKey"]
            ?? throw new Exception($"SystemSymmetricEncryptionKey not found in configuration"));

    protected override IEFConfiguration GetEFConfiguration()
        => new MySqlEFConfiguration(
            _configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception($"DefaultConnection string not found in configuration"));

    private class MySqlEFConfiguration(string connectionString) : EFMySqlConfigurationBase(connectionString)
    {
        public override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
                .LogTo(
                    filter: (eventId, logLevel) => eventId.Id == RelationalEventId.CommandExecuted.Id,
                    logger: SerilogEFEventDataWriter.Write);
#if DEBUG
            optionsBuilder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
#endif
        }
    }

    private class InMemoryConfig : EFInMemoryConfigurationBase
    {
        public override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseInMemoryDatabase("TestDb")
                .LogTo(
                    filter: (eventId, logLevel) => eventId.Id == RelationalEventId.CommandExecuted.Id,
                    logger: SerilogEFEventDataWriter.Write);
#if DEBUG
            optionsBuilder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
#endif
        }
    }

}
