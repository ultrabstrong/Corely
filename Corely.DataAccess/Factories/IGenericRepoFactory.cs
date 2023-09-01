using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories.AccountManagement;

namespace Corely.DataAccess.Factories
{
    public interface IGenericRepoFactory
    {
        public IAccountManagementRepoFactory CreateAccountManagementRepoFactory<T>(
            IDataAccessConnection<T> connectionProvider);
    }
}
