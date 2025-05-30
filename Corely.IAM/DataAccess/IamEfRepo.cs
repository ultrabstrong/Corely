﻿using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess;

// Extend EFRepo so we can specifically register the IAMDbContext
// otherwise DI container won't know which that the context should be used for EFRepo
internal sealed class IamEfRepo<T> : EFRepo<T>
    where T : class, IHasIdPk<int>
{
    public IamEfRepo(
        ILogger<IamEfRepo<T>> logger,
        IamDbContext dbContext)
        : base(logger, dbContext)
    {
    }
}
