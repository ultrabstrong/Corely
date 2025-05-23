﻿using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.IAM;

public abstract class EFServiceFactory(IServiceCollection serviceCollection, IConfiguration configuration)
    : ServiceFactoryBase(serviceCollection, configuration)

{
    protected sealed override void AddDataServices()
    {
        ServiceCollection.AddScoped(serviceProvider => GetEFConfiguration());
        ServiceCollection.AddDbContext<IamDbContext>();
        ServiceCollection.AddScoped(typeof(IReadonlyRepo<>), typeof(IamEfReadonlyRepo<>));
        ServiceCollection.AddScoped(typeof(IRepo<>), typeof(IamEfRepo<>));
        ServiceCollection.AddScoped<IUnitOfWorkProvider, IamEfUoWProvider>();
    }

    protected abstract IEFConfiguration GetEFConfiguration();
}
