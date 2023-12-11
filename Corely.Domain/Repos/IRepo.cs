namespace Corely.Domain.Repos
{
    public interface IRepo<T>
    {
        void Create(T entity);
        T? Get(int id);
        void Update(T entity);
        void Delete(T entity);
    }
}
