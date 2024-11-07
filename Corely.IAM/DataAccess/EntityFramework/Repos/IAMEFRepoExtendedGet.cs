using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess.EntityFramework.Repos
{
    internal sealed class IAMEFRepoExtendedGet<T> : EFRepoExtendedGet<T>
        where T : class, IHasIdPk
    {
        public IAMEFRepoExtendedGet(
            ILogger<EFRepoExtendedGet<T>> logger,
            IAMDbContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
