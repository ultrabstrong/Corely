using Corely.Domain.Enums;

namespace Corely.Domain.Models.Auth
{
    public record UpsertBasicAuthResult(
        bool IsSuccess,
        string? Message,
        int CreatedId,
        UpsertType UpsertType)
        : CreateResult(IsSuccess, Message, CreatedId)
    { }
}
