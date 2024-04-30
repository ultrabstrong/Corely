using Corely.DataAccess.EntityFramework.IAM;

namespace Corely.DataAccess.Factories
{
    public interface IGenericRepoFactory
    {
        public IIAMRepoFactory CreateAccountManagementRepoFactory();
    }
}
