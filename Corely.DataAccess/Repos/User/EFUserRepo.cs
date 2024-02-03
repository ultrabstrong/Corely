using Corely.DataAccess.DataSources.EntityFramework;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Repos.User
{
    internal sealed class EFUserRepo : EFRepoExtendedGetBase<UserEntity>
    {
        private readonly ILogger<EFUserRepo> _logger;
        private readonly AccountManagementDbContext _dbContext;

        protected override DbContext DbContext => _dbContext;
        protected override DbSet<UserEntity> Entities => _dbContext.Users;

        public EFUserRepo(
            ILogger<EFUserRepo> logger,
            AccountManagementDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogDebug("EFUserRepo created");
        }
    }
}
