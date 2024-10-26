namespace Corely.DataAccess.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkProvider
    {
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
