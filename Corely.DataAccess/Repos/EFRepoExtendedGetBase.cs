using Corely.DataAccess.Extensions;
using Corely.Domain.Entities;
using Corely.Domain.Repos;
using System.Linq.Expressions;

namespace Corely.DataAccess.Repos
{
    internal abstract class EFRepoExtendedGetBase<T>
        : EFRepoBase<T>, IRepoExtendedGet<T>
        where T : class, IHasIdPk
    {
        public async Task<T?> GetAsync(Expression<Func<T, bool>> query,
            Expression<Func<T, object>>? include = null)
        {
            return await Entities.GetAsync(query, include);
        }
    }
}
