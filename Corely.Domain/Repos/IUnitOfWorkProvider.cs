namespace Corely.Domain.Repos
{
    public interface IUnitOfWorkProvider
    {
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
