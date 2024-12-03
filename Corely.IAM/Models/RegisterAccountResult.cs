namespace Corely.IAM.Models;

public record RegisterAccountResult(
    bool IsSuccess,
    string? Message,
    int CreatedAccountId)
    : ResultBase(IsSuccess, Message);
