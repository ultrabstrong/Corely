using Corely.IAM.DataAccess.EntityFramework;

namespace Corely.IAM.DataAccess
{
    public interface IGenericRepoFactory
    {
        public IIAMRepoFactory CreateIAMRepoFactory();
    }
}
