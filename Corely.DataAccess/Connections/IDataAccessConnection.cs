namespace Corely.DataAccess.Connections
{
    public interface IDataAccessConnection<T>
    {
        string ConnectionName { get; }
        T GetConnection();
    }
}
