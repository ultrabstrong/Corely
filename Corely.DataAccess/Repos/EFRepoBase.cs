using Corely.Common.Models;
using Corely.Domain.Entities;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.Repos
{
    internal abstract class EFRepoBase<T>
        : DisposeBase, IRepo<T>
        where T : class, IHasIdPk
    {
        protected abstract DbContext DbContext { get; }
        protected abstract DbSet<T> Entities { get; }

        public virtual async Task<int> CreateAsync(T entity)
        {
            var newEntity = await Entities.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return newEntity.Entity.Id;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await Entities.FindAsync(id);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            Entities.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            Entities.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        protected override void DisposeManagedResources()
            => DbContext.Dispose();
    }
}
