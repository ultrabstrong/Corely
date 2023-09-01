namespace Corely.DataAccess.Connections
{
    public class DataAccessStringConnection : IDataAccessConnection<string>
    {
        private readonly string _connection;

        public string ConnectionName { get; private set; }

        public DataAccessStringConnection(string name, string connection)
        {
            ConnectionName = name;
            _connection = connection;
        }

        public string GetConnection() => _connection;
    }
}
