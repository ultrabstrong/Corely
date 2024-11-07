using Corely.DataAccess.EntityFramework;

namespace Corely.IAM.DataAccess.EntityFramework
{
    internal sealed class IAMEFUoWProvider : EFUoWProvider
    {
        public IAMEFUoWProvider(IAMDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
