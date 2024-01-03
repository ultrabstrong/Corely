namespace Corely.Domain.Repos
{
    public interface IRepo<T>
    {
        Task Create(T entity);
        Task<T?> Get(int id);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
