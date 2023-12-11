using Corely.DataAccess.DataSources.EntityFramework;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Corely.DataAccess.Repos.User
{
    internal class EFUserRepo : IUserRepo
    {
        private readonly ILogger _logger;
        private readonly AccountManagementDbContext _dbContext;

        public EFUserRepo(
            ILogger logger,
            AccountManagementDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.Debug("EFUserRepo created");
        }

        public void Create(UserEntity entity)
        {
            _dbContext.Users.Add(entity);
        }

        public void Delete(UserEntity entity)
        {
            _dbContext.Users.Remove(entity);
        }

        public UserEntity? Get(int id)
        {
            return _dbContext.Users.Find(id);
        }

        public UserEntity? GetByEmail(string email)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Email == email);
        }

        public UserEntity? GetByUserName(string userName)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Username == userName);
        }

        public UserEntity? GetWithDetailsByEmail(string email)
        {
            return _dbContext.Users
                .Include(u => u.BasicAuth)
                .FirstOrDefault(u => u.Email == email);
        }

        public UserEntity? GetWithDetailsById(int userId)
        {
            return _dbContext.Users
                .Include(u => u.BasicAuth)
                .FirstOrDefault(u => u.Id == userId);
        }

        public UserEntity? GetWithDetailsByUserName(string userName)
        {
            return _dbContext.Users
                .Include(u => u.BasicAuth)
                .FirstOrDefault(u => u.Username == userName);
        }

        public void Update(UserEntity entity)
        {
            _dbContext.Users.Update(entity);
        }

        public bool DoesUserExist(string userName, string email)
        {
            return _dbContext.Users
                .Any(u => u.Username == userName || u.Email == email);
        }
    }
}
