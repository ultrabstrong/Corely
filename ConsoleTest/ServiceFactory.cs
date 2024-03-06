using Corely.DataAccess;
using Corely.DataAccess.Connections;
using Corely.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ConsoleTest
{
    internal class ServiceFactory : ServiceFactoryBase
    {
        private class InMemoryConfig : IEFConfiguration
        {
            public void Configure(DbContextOptionsBuilder optionsBuilder)
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
            // Uncomment to use the Corely mock data access
            // var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");
            var connection = new DataAccessConnection<EFConnection>(ConnectionNames.EntityFramework, new EFConnection(new InMemoryConfig()));
            var keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, services);
            keyedDataServiceFactory.AddAllDataServices(services);
        }
    }
}
