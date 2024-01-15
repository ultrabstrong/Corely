using Corely.Common.Models;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Extensions;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Corely.DataAccess.Repos.User
{
    internal class EFUserRepo : DisposeBase, IRepoExtendedGet<UserEntity>
    {
        private readonly ILogger<EFUserRepo> _logger;
        private readonly AccountManagementDbContext _dbContext;

        public EFUserRepo(
            ILogger<EFUserRepo> logger,
            AccountManagementDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogDebug("EFUserRepo created");
        }

        public async Task CreateAsync(UserEntity entity)
        {
            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserEntity?> GetAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> query,
            Expression<Func<UserEntity, object>>? include = null)
        {
            return await _dbContext.Users.GetAsync(query, include);
        }

        public async Task UpdateAsync(UserEntity entity)
        {
            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserEntity entity)
        {
            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        protected override void DisposeManagedResources()
            => _dbContext.Dispose();

    }
}
