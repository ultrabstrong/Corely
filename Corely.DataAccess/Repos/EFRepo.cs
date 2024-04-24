using Corely.Common.Extensions;
using Corely.Common.Models;
using Corely.Domain.Entities;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Repos
{
    internal class EFRepo<T>
        : DisposeBase, IRepo<T>
        where T : class, IHasIdPk
    {
        protected readonly ILogger<EFRepo<T>> _logger;
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EFRepo(
            ILogger<EFRepo<T>> logger,
            DbContext dbContext,
            DbSet<T> dbSet)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _dbContext = dbContext.ThrowIfNull(nameof(dbContext));
            _dbSet = dbSet.ThrowIfNull(nameof(dbSet));
            _logger.LogDebug("{RepoType} created for {EntityType}", GetType(), typeof(T));
        }

        public virtual async Task<int> CreateAsync(T entity)
        {
            var newEntity = await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return newEntity.Entity.Id;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        protected override void DisposeManagedResources()
            => _dbContext.Dispose();
    }
}
