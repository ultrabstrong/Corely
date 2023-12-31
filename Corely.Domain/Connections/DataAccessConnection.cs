namespace Corely.Domain.Connections
{
    public class DataAccessConnection<T> : IDataAccessConnection<T>
    {
        private readonly T _connection;

        public string ConnectionName { get; private set; }

        public DataAccessConnection(string name, T connection)
        {
            _connection = connection;
            ConnectionName = name;
        }

        public T GetConnection() => _connection;
    }
}
