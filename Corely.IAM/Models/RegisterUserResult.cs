namespace Corely.IAM.Models;

public record RegisterUserResult(
    bool IsSuccess,
    string? Message,
    int CreatedUserId,
    int CreatedAuthId)
    : ResultBase(IsSuccess, Message);
