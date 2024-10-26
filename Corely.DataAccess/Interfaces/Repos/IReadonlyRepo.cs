namespace Corely.DataAccess.Interfaces.Repos
{
    public interface IReadonlyRepo<T> where T : class
    {
        Task<T?> GetAsync(int id);
    }
}
