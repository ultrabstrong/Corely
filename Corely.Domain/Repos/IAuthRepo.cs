namespace Corely.Domain.Repos
{
    public interface IAuthRepo<T> : IRepo<T>
    {
        T? GetByUserId(int userId);
        T? GetByUserName(string userName);
    }
}
