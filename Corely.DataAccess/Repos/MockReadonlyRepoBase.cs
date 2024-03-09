using Corely.Domain.Entities;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos
{
    internal abstract class MockReadonlyRepoBase<T>
        : IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        protected abstract List<T> Entities { get; }

        public virtual Task<T?> GetAsync(int id)
        {
            return Task.FromResult(Entities.FirstOrDefault(u => u.Id == id));
        }
    }
}
