using Corely.DataAccess.Repos.Accounts;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain;
using Corely.Domain.Connections;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
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

        protected override void AddDataAccessRepos(IServiceCollection services)
        {
            var connection = new DataAccessConnection<string>(
                ConnectionNames.Mock, "");

            // Keyed is used to allow multiple connections to be registered
            services.AddKeyedSingleton<IDataAccessConnection<string>>(
                connection.ConnectionName,
                (serviceProvider, key) => connection);

            services.AddTransient<IRepoExtendedGet<AccountEntity>, MockAccountRepo>();
            services.AddTransient<IRepoExtendedGet<UserEntity>, MockUserRepo>();
            services.AddTransient<IRepoExtendedGet<BasicAuthEntity>, MockBasicAuthRepo>();
        }
    }
}
