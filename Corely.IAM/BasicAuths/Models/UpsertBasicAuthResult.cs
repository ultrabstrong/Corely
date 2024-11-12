using Corely.IAM.Enums;
using Corely.IAM.Models;

namespace Corely.IAM.BasicAuths.Models
{
    public record UpsertBasicAuthResult(
        bool IsSuccess,
        string? Message,
        int CreatedId,
        UpsertType UpsertType)
        : CreateResult(IsSuccess, Message, CreatedId);
}
