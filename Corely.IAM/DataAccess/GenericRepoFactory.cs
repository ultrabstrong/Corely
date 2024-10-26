using Corely.Common.Extensions;
using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.Mock;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess
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

        public virtual IIAMRepoFactory CreateIAMRepoFactory()
        {
            return _connection.ConnectionName switch
            {
                ConnectionNames.EntityFramework =>
                    new EFIAMRepoFactory(
                        _loggerFactory,
                        ((IDataAccessConnection<EFConnection>)_connection).GetConnection()),
                ConnectionNames.Mock =>
                    new MockIAMRepoFactory(),
                _ =>
                    throw new ArgumentOutOfRangeException(_connection.ConnectionName),
            };
        }
    }
}
