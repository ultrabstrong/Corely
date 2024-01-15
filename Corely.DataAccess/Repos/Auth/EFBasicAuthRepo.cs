using Corely.Common.Extensions;
using Corely.Common.Models;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Extensions;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Corely.DataAccess.Repos.Auth
{
    internal class EFBasicAuthRepo : DisposeBase, IRepoExtendedGet<BasicAuthEntity>
    {
        private readonly ILogger<EFBasicAuthRepo> _logger;
        private readonly AccountManagementDbContext _dbContext;

        public EFBasicAuthRepo(
            ILogger<EFBasicAuthRepo> logger,
            AccountManagementDbContext dbContext)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _dbContext = dbContext.ThrowIfNull(nameof(dbContext));
            _logger.LogDebug("EFBasicAuthRepo created");
        }

        public async Task CreateAsync(BasicAuthEntity entity)
        {
            await _dbContext.BasicAuths.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<BasicAuthEntity?> GetAsync(int id)
        {
            return await _dbContext.BasicAuths.FindAsync(id);
        }

        public async Task<BasicAuthEntity?> GetAsync(Expression<Func<BasicAuthEntity, bool>> query,
            Expression<Func<BasicAuthEntity, object>>? include = null)
        {
            return await _dbContext.BasicAuths.GetAsync(query, include);
        }

        public async Task UpdateAsync(BasicAuthEntity entity)
        {
            _dbContext.BasicAuths.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(BasicAuthEntity entity)
        {
            _dbContext.BasicAuths.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        protected override void DisposeManagedResources()
            => _dbContext.Dispose();
    }
}
