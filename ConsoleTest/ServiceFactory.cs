using ConsoleTest.SerilogCustomization;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace ConsoleTest;

internal class ServiceFactory : EFServiceFactory
{
    private static readonly Lazy<ServiceFactory> _instance = new(() => new ServiceFactory());
    public static ServiceFactory Instance => _instance.Value;

    private ServiceFactory() { }

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

    protected override void AddLogging(ILoggingBuilder builder)
    {
        builder.AddSerilog(logger: Log.Logger, dispose: false);
    }

    protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
        => new SecurityConfigurationProvider();

    protected override IEFConfiguration GetEFConfiguration()
        => new MySqlEFConfiguration(ConfigurationProvider.GetConnectionString());

}
