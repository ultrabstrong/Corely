using Corely.DataAccess.Connections;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories
{
    public class GenericRepoFactory<T> : GenericRepoFactoryBase<T>
    {
        public GenericRepoFactory(
            ILoggerFactory loggerFactory,
            IDataAccessConnection<T> connection)
            : base(loggerFactory, connection) { }
    }
}
