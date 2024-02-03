using Corely.Domain.Entities;
using Corely.Domain.Repos;
using System.Linq.Expressions;

namespace Corely.DataAccess.Repos
{
    internal abstract class MockRepoExtendedGetBase<T>
        : MockRepoBase<T>, IRepoExtendedGet<T>
        where T : IHasIdPk
    {
        public virtual Task<T?> GetAsync(Expression<Func<T, bool>> query,
            Expression<Func<T, object>>? include = null)
        {
            ArgumentNullException.ThrowIfNull(query);
            var predicate = query.Compile();
            return Task.FromResult(Entities.FirstOrDefault(predicate));
        }
    }
}
