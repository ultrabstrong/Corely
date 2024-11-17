using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Corely.DataAccess.EntityFramework.Repos
{
    public class EFReadonlyRepo<T>
        : IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        protected readonly ILogger<EFReadonlyRepo<T>> Logger;
        protected readonly DbSet<T> DbSet;

        public EFReadonlyRepo(
            ILogger<EFReadonlyRepo<T>> logger,
            DbContext context)
        {
            Logger = logger.ThrowIfNull(nameof(logger));
            DbSet = context.Set<T>().ThrowIfNull(nameof(context));
            Logger.LogDebug("{RepoType} created for {EntityType}", GetType().Name.Split('`')[0], typeof(T).Name);
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<T?> GetAsync(
           Expression<Func<T, bool>> query,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
           Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            ArgumentNullException.ThrowIfNull(query);
            var queryable = DbSet.AsQueryable();

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

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> query)
        {
            ArgumentNullException.ThrowIfNull(query);
            return await DbSet.AnyAsync(query);
        }
    }
}
