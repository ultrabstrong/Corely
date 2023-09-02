namespace Corely.DataAccess.Connections
{
    public class DataAccessConnection<T> : IDataAccessConnection<T>
    {
        private readonly T _connection;

        public string ConnectionName { get; private set; }

        public DataAccessConnection(string name, T connection)
        {
            ConnectionName = name;
            _connection = connection;
        }

        public T GetConnection() => _connection;
    }
}
