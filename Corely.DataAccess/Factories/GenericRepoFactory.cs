using Corely.DataAccess.Connections;

namespace Corely.DataAccess.Factories
{
    public class GenericRepoFactory<T> : GenericRepoFactoryBase<T>
    {
        public GenericRepoFactory(IDataAccessConnection<T> connection)
            : base(connection)
        {
        }
    }
}
