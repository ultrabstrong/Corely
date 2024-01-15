using Corely.Common.Extensions;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Repos.Accounts
{
    internal class EFAccountRepo : EFRepoExtendedGetBase<AccountEntity>
    {
        private readonly ILogger<EFAccountRepo> _logger;
        private readonly AccountManagementDbContext _dbContext;

        protected override DbContext DbContext => _dbContext;
        protected override DbSet<AccountEntity> Entities => _dbContext.Accounts;

        public EFAccountRepo(
            ILogger<EFAccountRepo> logger,
            AccountManagementDbContext dbContext)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _dbContext = dbContext.ThrowIfNull(nameof(dbContext));
            _logger.LogDebug("EFAccountRepo created");
        }
    }
}
