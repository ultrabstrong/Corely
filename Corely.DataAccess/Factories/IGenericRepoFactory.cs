namespace Corely.DataAccess.Factories
{
    public interface IGenericRepoFactory
    {
        public IAccountManagementRepoFactory CreateAccountManagementRepoFactory();
    }
}
