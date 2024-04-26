using Corely.IAM.Enums;

namespace Corely.IAM.Models.Auth
{
    public record UpsertBasicAuthResult(
        bool IsSuccess,
        string? Message,
        int CreatedId,
        UpsertType UpsertType)
        : CreateResult(IsSuccess, Message, CreatedId)
    { }
}
