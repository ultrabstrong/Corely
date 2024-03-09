namespace Corely.Domain.Repos
{
    public interface IReadonlyRepo<T> where T : class
    {
        Task<T?> GetAsync(int id);
    }
}
