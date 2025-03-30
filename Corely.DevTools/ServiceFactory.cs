using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DevTools.SerilogCustomization;
using Corely.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace Corely.DevTools;

internal class ServiceFactory(IServiceCollection servicesCollection, IConfiguration configuration)
    : EFServiceFactory(servicesCollection, configuration)
{
    protected override void AddLogging(ILoggingBuilder builder)
        => builder.AddSerilog(logger: Log.Logger, dispose: false);

    protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
        => new SecurityConfigurationProvider(
            Configuration["SystemSymmetricEncryptionKey"]
            ?? throw new Exception($"SystemSymmetricEncryptionKey not found in configuration"));

    protected override IEFConfiguration GetEFConfiguration()
        => new MySqlEFConfiguration(
            Configuration.GetConnectionString("DataRepoConnection")
            ?? throw new Exception($"DataRepoConnection string not found in configuration"));

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
        }
    }

}
