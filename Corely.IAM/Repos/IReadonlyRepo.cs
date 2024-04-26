namespace Corely.IAM.Repos
{
    public interface IReadonlyRepo<T> where T : class
    {
        Task<T?> GetAsync(int id);
    }
}
