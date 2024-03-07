namespace Corely.Domain.Services
{
    public interface IEntityGetterService<T>
    {
        Task<T?> GetAsync(int id);
    }
}
