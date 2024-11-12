using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess.EntityFramework.Repos
{
    // Extend EFRepo so we can specifically register the IAMDbContext
    // otherwise DI container won't know which that the context should be used for EFRepo
    internal sealed class IamEfReadonlyRepo<T> : EFReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        public IamEfReadonlyRepo(
            ILogger<IamEfReadonlyRepo<T>> logger,
            IamDbContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
