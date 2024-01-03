using Corely.Common.Extensions;
using Corely.Common.Models;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Repos.Auth
{
    internal class EFBasicAuthRepo : DisposeBase, IAuthRepo<BasicAuthEntity>
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

        public async Task Create(BasicAuthEntity entity)
        {
            await _dbContext.BasicAuths.AddAsync(entity);
        }

        public async Task<BasicAuthEntity?> Get(int id)
        {
            return await _dbContext.BasicAuths.FindAsync(id);
        }

        public async Task<BasicAuthEntity?> GetByUserId(int userId)
        {
            return await _dbContext.BasicAuths
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<BasicAuthEntity?> GetByUserName(string userName)
        {
            return await _dbContext.BasicAuths
                .FirstOrDefaultAsync(a => a.Username == userName);
        }

        public async Task Update(BasicAuthEntity entity)
        {
            _dbContext.BasicAuths.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Delete(BasicAuthEntity entity)
        {
            _dbContext.BasicAuths.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        protected override void DisposeManagedResources()
        {
            _dbContext.Dispose();
        }
    }
}
