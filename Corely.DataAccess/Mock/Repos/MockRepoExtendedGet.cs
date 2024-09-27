using Corely.IAM.Entities;
using Corely.IAM.Repos;
using System.Linq.Expressions;

namespace Corely.DataAccess.Mock.Repos
{
    internal class MockRepoExtendedGet<T>
        : MockRepo<T>, IRepoExtendedGet<T>
        where T : IHasIdPk
    {
        public virtual Task<T?> GetAsync(Expression<Func<T, bool>> query,
            Expression<Func<T, object>>? include = null)
        {
            ArgumentNullException.ThrowIfNull(query);
            var predicate = query.Compile();
            return Task.FromResult(Entities.FirstOrDefault(predicate));
        }

        public virtual async Task<T?> GetAsync(
            Expression<Func<T, bool>> query,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            ArgumentNullException.ThrowIfNull(query);
            var predicate = query.Compile();
            var queryable = Entities.AsQueryable();

            if (include != null)
            {
                queryable = include(queryable);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return await Task.FromResult(queryable.FirstOrDefault(predicate));
        }
    }
}
