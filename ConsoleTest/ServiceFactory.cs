using Corely.DataAccess;
using Corely.Domain;
using Corely.Domain.Connections;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ConsoleTest
{
    internal class ServiceFactory : ServiceFactoryBase
    {
        protected override void AddLogger(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddSerilog(logger: Log.Logger, dispose: false));
        }

        protected override void AddDataAccessServices(IServiceCollection services)
        {
            var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");
            var keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, services);
            keyedDataServiceFactory.AddAllDataServices(services);
        }
    }
}
