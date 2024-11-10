using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.EntityFramework.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.IAM
{
    public abstract class EFServiceFactory : ServiceFactoryBase
    {
        protected override void AddDataServices(IServiceCollection services)
        {
            services.AddScoped(serviceProvider => GetEFConfiguraiton());
            services.AddDbContext<IAMDbContext>();
            // Todo : Explore adding context-specific repos to allow for multiple contexts
            services.AddScoped(typeof(IRepo<>), typeof(IAMEFRepo<>));
            services.AddScoped(typeof(IRepoExtendedGet<>), typeof(IAMEFRepoExtendedGet<>));
            services.AddScoped(typeof(IReadonlyRepo<>), typeof(IAMEFReadonlyRepo<>));
            services.AddScoped<IUnitOfWorkProvider, IAMEFUoWProvider>();
        }

        protected abstract IEFConfiguration GetEFConfiguraiton();
    }
}
