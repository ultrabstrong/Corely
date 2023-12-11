using Corely.DataAccess.DataSources.EntityFramework;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Corely.Shared.Extensions;
using Serilog;

namespace Corely.DataAccess.Repos.Auth
{
    internal class EFBasicAuthRepo : IAuthRepo<BasicAuthEntity>
    {
        private readonly ILogger _logger;
        private readonly AccountManagementDbContext _dbContext;

        public EFBasicAuthRepo(
            ILogger logger,
            AccountManagementDbContext dbContext)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            _dbContext = dbContext.ThrowIfNull(nameof(dbContext));
            _logger.Debug("EFBasicAuthRepo created");
        }

        public void Create(BasicAuthEntity entity)
        {
            _dbContext.BasicAuths.Add(entity);
        }

        public void Delete(BasicAuthEntity entity)
        {
            _dbContext.BasicAuths.Remove(entity);
        }

        public BasicAuthEntity? Get(int id)
        {
            return _dbContext.BasicAuths.Find(id);
        }

        public BasicAuthEntity? GetByUserId(int userId)
        {
            return _dbContext.BasicAuths
                .FirstOrDefault(a => a.UserId == userId);
        }

        public BasicAuthEntity? GetByUserName(string userName)
        {
            return _dbContext.BasicAuths
                .FirstOrDefault(a => a.Username == userName);
        }

        public void Update(BasicAuthEntity entity)
        {
            _dbContext.BasicAuths.Update(entity);
        }
    }
}
