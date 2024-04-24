using Corely.Common.Extensions;
using Corely.Domain.Entities;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Repos
{
    internal class EFReadonlyRepo<T>
        : IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        protected readonly ILogger<EFReadonlyRepo<T>> _logger;
        protected readonly DbSet<T> _dbSet;

        public EFReadonlyRepo(
            ILogger<EFReadonlyRepo<T>> logger,
            DbSet<T> dbSet)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _dbSet = dbSet.ThrowIfNull(nameof(dbSet));
            _logger.LogDebug("{RepoType} created for {EntityType}", GetType(), typeof(T));
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
