using Corely.Domain.Entities;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Corely.DataAccess.EntityFramework.Repos
{
    internal class EFRepoExtendedGet<T>
        : EFRepo<T>, IRepoExtendedGet<T>
        where T : class, IHasIdPk
    {
        public EFRepoExtendedGet(
            ILogger<EFRepoExtendedGet<T>> logger,
            DbContext dbContext,
            DbSet<T> dbSet)
            : base(logger, dbContext, dbSet)
        {
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> query,
            Expression<Func<T, object>>? include = null)
        {
            return await _dbSet.GetAsync(query, include);
        }
    }
}
