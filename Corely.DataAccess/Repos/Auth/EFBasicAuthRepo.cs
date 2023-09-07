using Corely.DataAccess.DataAccess.EntityFramework;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
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
            _logger = logger;
            _dbContext = dbContext;
        }

        public void Create(BasicAuthEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BasicAuthEntity entity)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public void Update(BasicAuthEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
