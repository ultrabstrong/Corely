using Corely.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Corely.DataAccess.Extensions
{
    internal static class DbSetExtensions
    {
        public static async Task<T?> GetAsync<T>(this DbSet<T> dbSet,
            Expression<Func<T, bool>> query,
            Expression<Func<T, object>>? include = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(dbSet, nameof(dbSet));
            IQueryable<T> queryable = dbSet.ThrowIfNull(nameof(dbSet));

            if (include != null)
            {
                queryable = queryable.Include(include);
            }

            return await queryable.FirstOrDefaultAsync(query);
        }
    }
}
