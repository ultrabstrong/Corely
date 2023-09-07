﻿using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories.AccountManagement;
using Serilog;

namespace Corely.DataAccess.Factories
{
    public abstract class GenericRepoFactoryBase<T> : IGenericRepoFactory<T>
    {
        protected readonly ILogger _logger;
        private readonly IDataAccessConnection<T> _connection;

        protected internal GenericRepoFactoryBase(
            ILogger logger,
            IDataAccessConnection<T> connection)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));
            ArgumentException.ThrowIfNullOrWhiteSpace(connection.ConnectionName, nameof(connection.ConnectionName));

            _logger = logger;
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
                    new EfMySqlAccountManagementRepoFactory(
                        _logger,
                        ((IDataAccessConnection<string>)_connection).GetConnection()),
                _ =>
                    throw new ArgumentOutOfRangeException(_connection.ConnectionName),
            };
        }
    }
}