using Corely.Common.Models;
using Corely.IAM.Repos;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.EntityFramework
{
    internal class EFUoWProvider : DisposeBase, IUnitOfWorkProvider
    {
        private readonly DbContext _dbContext;

        public EFUoWProvider(DbContext dbContext)
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
            => _dbContext?.Dispose();
    }
}
