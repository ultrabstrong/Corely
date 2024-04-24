using Corely.DataAccess;
using Corely.DataAccess.Connections;
using Corely.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests
{
    public class ServiceFactory : ServiceFactoryBase
    {
        private class InMemoryConfig : InMemoryEFConfigurationBase
        {
            public override void Configure(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase("TestDb");
            }
        }

        public ServiceFactory() : base() { }

        protected override void AddLogger(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }

        protected override void AddDataAccessServices(IServiceCollection services)
        {
            // Corely mock db connection
            var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");

            // EF in memory db connection
            //var connection = new DataAccessConnection<EFConnection>(ConnectionNames.EntityFramework, new EFConnection(new InMemoryConfig()));

            var keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, services);
            keyedDataServiceFactory.AddAllDataServices(services);
        }
    }
}
