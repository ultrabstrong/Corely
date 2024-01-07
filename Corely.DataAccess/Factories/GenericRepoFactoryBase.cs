using Corely.Common.Extensions;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Connections;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories
{
    public abstract class GenericRepoFactoryBase<T> : IGenericRepoFactory<T>
    {
        protected readonly ILoggerFactory _loggerFactory;
        private readonly IDataAccessConnection<T> _connection;

        protected internal GenericRepoFactoryBase(
            ILoggerFactory loggerFactory,
            IDataAccessConnection<T> connection)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connection.ConnectionName, nameof(connection.ConnectionName));

            _loggerFactory = loggerFactory.ThrowIfNull(nameof(loggerFactory));
            _connection = connection.ThrowIfNull(nameof(connection));
            CheckKnownConnectionDataTypes();
        }

        protected internal virtual void CheckKnownConnectionDataTypes()
        {
            switch (_connection.ConnectionName)
            {
                case ConnectionNames.EntityFrameworkMySql:
                    ThrowForInvalidDataType<string>();
                    break;
                case ConnectionNames.Mock:
                    // Mock does not require a connection data type because there is no connection
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
                ConnectionNames.EntityFrameworkMySql =>
                    new EfMySqlAccountManagementRepoFactory(
                        _loggerFactory,
                        ((IDataAccessConnection<string>)_connection).GetConnection()),
                ConnectionNames.Mock =>
                    new MockAccountManagementRepoFactory(),
                _ =>
                    throw new ArgumentOutOfRangeException(_connection.ConnectionName),
            };
        }
    }
}
