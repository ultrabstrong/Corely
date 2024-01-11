using Corely.Common.Models;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Repos.User
{
    internal class EFUserRepo : DisposeBase, IUserRepo
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

        public async Task Create(UserEntity entity)
        {
            await _dbContext.Users.AddAsync(entity);
        }

        public async Task<UserEntity?> Get(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<UserEntity?> GetByEmail(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity?> GetByUserName(string userName)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == userName);
        }

        public async Task<UserEntity?> GetByUserNameOrEmail(string userName, string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == userName || u.Email == email);
        }

        public async Task<UserEntity?> GetWithDetailsByEmail(string email)
        {
            return await _dbContext.Users
                .Include(u => u.BasicAuth)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity?> GetWithDetailsById(int userId)
        {
            return await _dbContext.Users
                .Include(u => u.BasicAuth)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<UserEntity?> GetWithDetailsByUserName(string userName)
        {
            return await _dbContext.Users
                .Include(u => u.BasicAuth)
                .FirstOrDefaultAsync(u => u.Username == userName);
        }

        public async Task Update(UserEntity entity)
        {
            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(UserEntity entity)
        {
            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        protected override void DisposeManagedResources()
        {
            _dbContext.Dispose();
        }
    }
}
