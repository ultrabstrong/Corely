using Corely.Domain.Connections;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories
{
    public class GenericRepoFactory<T>(
        ILoggerFactory loggerFactory,
        IDataAccessConnection<T> connection)
        : GenericRepoFactoryBase<T>(loggerFactory, connection)
    {
    }
}
