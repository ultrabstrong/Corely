namespace Corely.Domain.Services
{
    public interface IEntityReadonlyService<T>
    {
        Task<T?> GetAsync(int id);
    }
}
