using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.IAM;

public abstract class EFServiceFactory : ServiceFactoryBase
{
    protected override void AddDataServices(IServiceCollection services)
    {
        services.AddScoped(serviceProvider => GetEFConfiguration());
        services.AddDbContext<IamDbContext>();
        services.AddScoped(typeof(IReadonlyRepo<>), typeof(IamEfReadonlyRepo<>));
        services.AddScoped(typeof(IRepo<>), typeof(IamEfRepo<>));
        services.AddScoped<IUnitOfWorkProvider, IamEfUoWProvider>();
    }

    protected abstract IEFConfiguration GetEFConfiguration();
}
