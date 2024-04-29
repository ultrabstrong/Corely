using Corely.Common.Extensions;
using Corely.IAM.Entities;
using Corely.IAM.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.EntityFramework.Repos
{
    internal class EFRepo<T>
        : IRepo<T>
        where T : class, IHasIdPk
    {
        private readonly Func<Task> _saveChangesAsync;

        protected readonly ILogger<EFRepo<T>> _logger;
        protected readonly DbSet<T> _dbSet;

        public EFRepo(
            ILogger<EFRepo<T>> logger,
            Func<Task> saveChangesAsync,
            DbSet<T> dbSet)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _saveChangesAsync = saveChangesAsync.ThrowIfNull(nameof(saveChangesAsync));
            _dbSet = dbSet.ThrowIfNull(nameof(dbSet));
            _logger.LogDebug("{RepoType} created for {EntityType}", GetType().Name.Split('`')[0], typeof(T).Name);
        }

        public virtual async Task<int> CreateAsync(T entity)
        {
            var newEntity = await _dbSet.AddAsync(entity);
            await _saveChangesAsync();
            return newEntity.Entity.Id;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var existingEntity = _dbSet.Local.FirstOrDefault(m => m.Id == entity.Id);
            if (existingEntity == null)
            {
                // attach new entity instance to local context for tracking
                _dbSet.Attach(entity);
                _dbSet.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                // update existing tracked entity instance with new entity values
                _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            await _saveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _saveChangesAsync();
        }
    }
}
