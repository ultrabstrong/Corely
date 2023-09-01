using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories.AccountManagement;

namespace Corely.DataAccess.Factories
{
    public abstract class GenericRepoFactoryBase : IGenericRepoFactory
    {
        public virtual IAccountManagementRepoFactory CreateAccountManagementRepoFactory<T>(
            IDataAccessConnection<T> connection)
        {
            CheckKnownConnectionDataTypes(connection);

            return connection.ConnectionName switch
            {
                ConnectionName.EntityFramework =>
                    new EFAccountManagementRepoFactory(((IDataAccessConnection<string>)connection).GetConnection()),
                _ =>
                    throw new ArgumentOutOfRangeException(connection.ConnectionName),
            };
        }

        public virtual void CheckKnownConnectionDataTypes<T>(IDataAccessConnection<T> connection)
        {
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(connection.ConnectionName, nameof(connection.ConnectionName));

            switch (connection.ConnectionName)
            {
                case ConnectionName.EntityFramework:
                    ThrowForInvalidDataType<string, T>(connection);
                    break;
                default:
                    break;
            }
        }

        public virtual void ThrowForInvalidDataType<T1, T2>(IDataAccessConnection<T2> connection)
        {
            if (typeof(T1) != typeof(T2))
            {
                throw new ArgumentException($"Invalid connection type for {connection.ConnectionName}. Expected {typeof(T1)} and received {typeof(T2)}");
            }
        }
    }
}
