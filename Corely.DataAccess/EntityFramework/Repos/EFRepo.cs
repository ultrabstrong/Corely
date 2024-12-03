using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.EntityFramework.Repos;

public class EFRepo<T>
    : EFReadonlyRepo<T>, IRepo<T>
    where T : class, IHasIdPk
{
    protected readonly DbContext DbContext;

    public EFRepo(
        ILogger<EFRepo<T>> logger,
        DbContext dbContext)
        : base(logger, dbContext)
    {
        DbContext = dbContext.ThrowIfNull(nameof(dbContext));
        Logger.LogDebug("{RepoType} created for {EntityType}", GetType().Name.Split('`')[0], typeof(T).Name);
    }

    public virtual async Task<int> CreateAsync(T entity)
    {
        var newEntity = await DbSet.AddAsync(entity);
        await DbContext.SaveChangesAsync();
        return newEntity.Entity.Id;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        if (typeof(IHasModifiedUtc).IsAssignableFrom(typeof(T)))
        {
            ((IHasModifiedUtc)entity).ModifiedUtc = DateTime.UtcNow;
        }

        var existingEntity = DbSet.Local.FirstOrDefault(m => m.Id == entity.Id);
        if (existingEntity == null)
        {
            // attach new entity instance to local context for tracking
            DbSet.Attach(entity).State = EntityState.Modified;
        }
        else
        {
            // update existing tracked entity instance with new entity values
            DbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
        }
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        if (!await DbSet.AnyAsync(e => e.Id == entity.Id))
        {
            return;
        }
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null)
        {
            return;
        }
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
    }
}
