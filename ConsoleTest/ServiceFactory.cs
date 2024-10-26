using ConsoleTest.SerilogCustomization;
using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM;
using Corely.IAM.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace ConsoleTest
{
    internal class ServiceFactory : ServiceFactoryBase
    {
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

        protected override void AddLogger(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddSerilog(logger: Log.Logger, dispose: false));
        }

        protected override void AddDataAccessServices(IServiceCollection services)
        {
            // Corely mock db connection
            // var connection = new DataAccessConnection<string>(ConnectionNames.Mock, string.Empty);

            // EF in memory db connection
            //var connection = new DataAccessConnection<EFConnection>(ConnectionNames.EntityFramework, new EFConnection(new InMemoryConfig()));

            // EF mysql db connection
            var connection = new DataAccessConnection<EFConnection>(
                ConnectionNames.EntityFramework,
                new EFConnection(new MySqlEFConfiguration(ConfigurationProvider.GetConnectionString())));

            var keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, services);
            keyedDataServiceFactory.AddAllDataServices(services);
        }

        protected override void AddSecurityConfigurationProvider(IServiceCollection services)
        {
            services.AddScoped<ISecurityConfigurationProvider, SecurityConfigurationProvider>();
        }
    }
}
