using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess.EntityFramework.Repos
{
    internal sealed class IAMEFReadonlyRepo<T> : EFReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        public IAMEFReadonlyRepo(
            ILogger<EFReadonlyRepo<T>> logger,
            IAMDbContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
