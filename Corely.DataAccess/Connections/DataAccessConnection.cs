namespace Corely.DataAccess.Connections
{
    public class DataAccessConnection<T> : IDataAccessConnection<T>
    {
        private readonly T _connection;

        public string ConnectionName { get; private set; }

        public DataAccessConnection(string name, T connection)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            ConnectionName = name;
            _connection = connection;

            CheckKnownConnectionDataTypes();
        }

        protected internal virtual void CheckKnownConnectionDataTypes()
        {
            switch (ConnectionName)
            {
                case ConnectionNames.EntityFramework:
                    ThrowForInvalidDataType<EFConnection>();
                    break;
                case ConnectionNames.Mock:
                    // Mock does not connect to anything
                    break;
                default:
                    break;
            }
        }

        protected internal virtual void ThrowForInvalidDataType<T1>()
        {
            if (!typeof(T1).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException($"Invalid connection type for {ConnectionName}. Expected {typeof(T1)} and received {typeof(T)}");
            }
        }

        public T GetConnection() => _connection;
    }
}
