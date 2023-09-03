using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories.AccountManagement;

namespace Corely.DataAccess.Factories
{
    public abstract class GenericRepoFactoryBase<T> : IGenericRepoFactory<T>
    {
        private readonly IDataAccessConnection<T> _connection;

        protected internal GenericRepoFactoryBase(IDataAccessConnection<T> connection)
        {
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));
            ArgumentException.ThrowIfNullOrWhiteSpace(connection.ConnectionName, nameof(connection.ConnectionName));

            _connection = connection;
            CheckKnownConnectionDataTypes();
        }

        protected internal virtual void CheckKnownConnectionDataTypes()
        {
            switch (_connection.ConnectionName)
            {
                case ConnectionName.EntityFrameworkMySql:
                    ThrowForInvalidDataType<string>();
                    break;
                default:
                    break;
            }
        }

        protected internal virtual void ThrowForInvalidDataType<T1>()
        {
            if (typeof(T1) != typeof(T))
            {
                throw new ArgumentException($"Invalid connection type for {_connection.ConnectionName}. Expected {typeof(T1)} and received {typeof(T)}");
            }
        }

        public virtual IAccountManagementRepoFactory CreateAccountManagementRepoFactory()
        {
            return _connection.ConnectionName switch
            {
                ConnectionName.EntityFrameworkMySql =>
                    new EfMySqlAccountManagementRepoFactory(((IDataAccessConnection<string>)_connection).GetConnection()),
                _ =>
                    throw new ArgumentOutOfRangeException(_connection.ConnectionName),
            };
        }
    }
}
