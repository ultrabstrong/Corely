using Corely.IAM.Enums;
using Corely.IAM.Models;

namespace Corely.IAM.Auth.Models
{
    public record UpsertBasicAuthResult(
        bool IsSuccess,
        string? Message,
        int CreatedId,
        UpsertType UpsertType)
        : CreateResult(IsSuccess, Message, CreatedId)
    { }
}
