using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.DataAccess.Mock;
using Corely.DataAccess.Mock.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.IAM
{
    public abstract class MockDbServiceFactory : ServiceFactoryBase
    {
        protected override void AddDataServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IRepo<>), typeof(MockRepo<>));
            services.AddSingleton(typeof(IRepoExtendedGet<>), typeof(MockRepoExtendedGet<>));
            services.AddSingleton(typeof(IReadonlyRepo<>), typeof(MockReadonlyRepo<>));
            services.AddSingleton<IUnitOfWorkProvider, MockUoWProvider>();
        }
    }
}
