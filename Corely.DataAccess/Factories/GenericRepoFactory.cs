using Corely.Domain.Connections;
using Serilog;

namespace Corely.DataAccess.Factories
{
    public class GenericRepoFactory<T>(
        ILogger logger,
        IDataAccessConnection<T> connection) : GenericRepoFactoryBase<T>(logger, connection)
    {
    }
}
