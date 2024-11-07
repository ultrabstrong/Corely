﻿using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Corely.DataAccess.EntityFramework.Repos
{
    public class EFRepoExtendedGet<T>
        : EFRepo<T>, IRepoExtendedGet<T>
        where T : class, IHasIdPk
    {
        public EFRepoExtendedGet(
            ILogger<EFRepoExtendedGet<T>> logger,
            DbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>> query,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            var queryable = _dbSet.AsQueryable();

            if (include != null)
            {
                queryable = include(queryable);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return await queryable.FirstOrDefaultAsync(query);
        }
    }
}
