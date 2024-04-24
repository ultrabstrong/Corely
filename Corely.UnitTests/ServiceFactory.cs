using Corely.DataAccess;
using Corely.DataAccess.Connections;
using Corely.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests
{
    public class ServiceFactory : ServiceFactoryBase
    {
        public ServiceFactory() : base() { }

        protected override void AddLogger(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }

        protected override void AddDataAccessServices(IServiceCollection services)
        {
            var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");
            var keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, services);
            keyedDataServiceFactory.AddAllDataServices(services);
        }
    }
}
