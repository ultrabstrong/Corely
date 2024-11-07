using Corely.Common.Models;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Corely.DataAccess.EntityFramework
{
    public class EFUoWProvider : DisposeBase, IUnitOfWorkProvider
    {
        private readonly DbContext _dbContext;
        private readonly bool _supportTransactions;
        private IDbContextTransaction? _transaction;

        public EFUoWProvider(DbContext dbContext)
        {
            _dbContext = dbContext;
            _supportTransactions = dbContext.Database.ProviderName
                != "Microsoft.EntityFrameworkCore.InMemory";
        }

        public async Task BeginAsync()
        {
            if (_transaction == null && _supportTransactions)
            {
                _transaction = await _dbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        protected override void DisposeManagedResources()
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();
        }

        protected async override ValueTask DisposeAsyncCore()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync();
            }
        }
    }
}
