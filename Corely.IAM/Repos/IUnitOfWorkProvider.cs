namespace Corely.IAM.Repos
{
    public interface IUnitOfWorkProvider
    {
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
