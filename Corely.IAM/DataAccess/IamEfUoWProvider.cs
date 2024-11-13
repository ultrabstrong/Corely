using Corely.DataAccess.EntityFramework;

namespace Corely.IAM.DataAccess
{
    internal sealed class IamEfUoWProvider : EFUoWProvider
    {
        public IamEfUoWProvider(IamDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
