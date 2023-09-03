using Corely.DataAccess.Factories.AccountManagement;

namespace Corely.DataAccess.Factories
{
    public interface IGenericRepoFactory<T>
    {
        public IAccountManagementRepoFactory CreateAccountManagementRepoFactory();
    }
}
