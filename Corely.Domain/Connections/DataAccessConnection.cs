namespace Corely.Domain.Connections
{
    public class DataAccessConnection<T>(
        string name,
        T connection)
        : IDataAccessConnection<T>
    {
        private readonly T _connection = connection;

        public string ConnectionName { get; private set; } = name;

        public T GetConnection() => _connection;
    }
}
