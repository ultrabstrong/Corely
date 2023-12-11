namespace Corely.Domain.Repos
{
    public interface IRepo<T>
    {
        T? Get(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
