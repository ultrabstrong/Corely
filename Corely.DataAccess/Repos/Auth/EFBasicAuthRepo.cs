using Corely.Common.Extensions;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Repos.Auth
{
    internal class EFBasicAuthRepo : EFRepoExtendedGetBase<BasicAuthEntity>
    {
        private readonly ILogger<EFBasicAuthRepo> _logger;
        private readonly AccountManagementDbContext _dbContext;

        protected override DbContext DbContext => _dbContext;
        protected override DbSet<BasicAuthEntity> Entities => _dbContext.BasicAuths;

        public EFBasicAuthRepo(
            ILogger<EFBasicAuthRepo> logger,
            AccountManagementDbContext dbContext)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _dbContext = dbContext.ThrowIfNull(nameof(dbContext));
            _logger.LogDebug("EFBasicAuthRepo created");
        }
    }
}
