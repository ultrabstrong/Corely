using Corely.Common.Providers.Security.Password;
using Corely.Domain.Enums;

namespace Corely.Domain.Models.Auth
{
    public record UpsertBasicAuthResult(
        bool IsSuccess,
        string? Message,
        int CreatedId,
        UpsertType UpsertType,
        ValidatePasswordResult[] ValidatePasswordResults)
        : CreateResult(IsSuccess, Message, CreatedId)
    { }
}
