using Corely.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corely.DataAccess.DataSources.Mock
{
    public class MockUoWProvider : IUnitOfWorkProvider
    {
        public Task BeginAsync()
        {
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }
    }
}
