using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess.EntityFramework.Repos
{
    internal sealed class IAMEFRepo<T> : EFRepo<T>
        where T : class, IHasIdPk
    {
        public IAMEFRepo(
            ILogger<EFRepo<T>> logger,
            IAMDbContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
