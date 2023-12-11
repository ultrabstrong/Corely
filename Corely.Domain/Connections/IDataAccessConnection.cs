namespace Corely.Domain.Connections
{
    public interface IDataAccessConnection<T>
    {
        string ConnectionName { get; }
        T GetConnection();
    }
}
