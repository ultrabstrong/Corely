namespace Corely.Domain.Repos
{
    public interface IAuthRepo<T> : IRepo<T>
    {
        Task<T?> GetByUserId(int userId);
        Task<T?> GetByUserName(string userName);
    }
}
