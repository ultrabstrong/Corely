namespace Corely.Domain.Models
{
    public abstract record ResultBase(
        bool IsSuccess,
        string? Message)
    { }
}
