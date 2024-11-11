using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.EntityFramework.Repos
{
    public class EFRepo<T>
        : IRepo<T>
        where T : class, IHasIdPk
    {
        protected readonly ILogger<EFRepo<T>> _logger;
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EFRepo(
            ILogger<EFRepo<T>> logger,
            DbContext dbContext)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _dbContext = dbContext.ThrowIfNull(nameof(dbContext));
            _dbSet = dbContext.Set<T>();
            _logger.LogDebug("{RepoType} created for {EntityType}", GetType().Name.Split('`')[0], typeof(T).Name);
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
            if (typeof(IHasModifiedUtc).IsAssignableFrom(typeof(T)))
            {
                ((IHasModifiedUtc)entity).ModifiedUtc = DateTime.UtcNow;
            }

            var existingEntity = _dbSet.Local.FirstOrDefault(m => m.Id == entity.Id);
            if (existingEntity == null)
            {
                // attach new entity instance to local context for tracking
                _dbSet.Attach(entity).State = EntityState.Modified;
            }
            else
            {
                // update existing tracked entity instance with new entity values
                _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            if (!await _dbSet.AnyAsync(e => e.Id == entity.Id))
            {
                return;
            }
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return;
            }
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
