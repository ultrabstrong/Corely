using Corely.DataAccess.Connections;
using Serilog;

namespace Corely.DataAccess.Factories
{
    public class GenericRepoFactory<T> : GenericRepoFactoryBase<T>
    {
        public GenericRepoFactory(
            ILogger logger,
            IDataAccessConnection<T> connection)
            : base(logger, connection)
        {
        }
    }
}
