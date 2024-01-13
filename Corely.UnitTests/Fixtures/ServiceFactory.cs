using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Fixtures
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
            services.AddTransient<IAuthRepo<BasicAuthEntity>, MockBasicAuthRepo>();
            services.AddTransient<IUserRepo, MockUserRepo>();
        }
    }
}
