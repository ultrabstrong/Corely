namespace Corely.DataAccess.Interfaces.Entities;

public interface IHasIdPk<TKey>
{
    TKey Id { get; set; }
}
