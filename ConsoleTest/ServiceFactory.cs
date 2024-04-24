using Corely.DataAccess;
using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.Domain;
using Microsoft.EntityFrameworkCore;
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
                optionsBuilder.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
                optionsBuilder.LogTo(
                    Log.Logger.Debug,
                    new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuted }
                );
            }
        }

        private class InMemoryConfig : EFInMemoryConfigurationBase
        {
            public override void Configure(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase("TestDb");
                optionsBuilder.LogTo(
                    Log.Logger.Debug,
                    new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuted }
                );
            }
        }

        protected override void AddLogger(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddSerilog(logger: Log.Logger, dispose: false));
        }

        protected override void AddDataAccessServices(IServiceCollection services)
        {
            // Corely mock db connection
            // var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");

            // EF in memory db connection
            //var connection = new DataAccessConnection<EFConnection>(ConnectionNames.EntityFramework, new EFConnection(new InMemoryConfig()));

            // EF mysql db connection
            var connection = new DataAccessConnection<EFConnection>(
                ConnectionNames.EntityFramework,
                new EFConnection(new MySqlEFConfiguration(ConfigurationProvider.GetConnectionString())));

            var keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, services);
            keyedDataServiceFactory.AddAllDataServices(services);
        }
    }
}
