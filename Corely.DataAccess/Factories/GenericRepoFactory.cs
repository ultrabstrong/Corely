using Corely.Common.Extensions;
using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.AccountManagement;
using Corely.DataAccess.Mock;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.Factories
{
    public class GenericRepoFactory<T> : IGenericRepoFactory
    {
        protected readonly ILoggerFactory _loggerFactory;
        private readonly IDataAccessConnection<T> _connection;

        protected internal GenericRepoFactory(
            ILoggerFactory loggerFactory,
            IDataAccessConnection<T> connection)
        {
            _loggerFactory = loggerFactory.ThrowIfNull(nameof(loggerFactory));
            _connection = connection.ThrowIfNull(nameof(connection));
        }

        public virtual IAccountManagementRepoFactory CreateAccountManagementRepoFactory()
        {
            return _connection.ConnectionName switch
            {
                ConnectionNames.EntityFramework =>
                    new EFAccountManagementRepoFactory(
                        _loggerFactory,
                        ((IDataAccessConnection<EFConnection>)_connection).GetConnection()),
                ConnectionNames.Mock =>
                    new MockAccountManagementRepoFactory(),
                _ =>
                    throw new ArgumentOutOfRangeException(_connection.ConnectionName),
            };
        }
    }
}
