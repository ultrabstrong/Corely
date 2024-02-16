using Corely.Common.Models;
using Corely.Domain.Repos;

namespace Corely.DataAccess.DataSources.EntityFramework
{
    internal class EFAccountManagementUoWProvider : DisposeBase, IUnitOfWorkProvider
    {
        private readonly AccountManagementDbContext _dbContext;

        public EFAccountManagementUoWProvider(AccountManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        protected override void DisposeManagedResources()
        {
            _dbContext.Dispose();
        }
    }
}
