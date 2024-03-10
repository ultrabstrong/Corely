using Corely.Common.Models;
using Corely.Domain.Entities;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.Repos
{
    internal abstract class EFReadonlyRepoBase<T>
        : DisposeBase, IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        protected abstract DbContext DbContext { get; }
        protected abstract DbSet<T> Entities { get; }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await Entities.FindAsync(id);
        }

        protected override void DisposeManagedResources()
            => DbContext?.Dispose();
    }
}
