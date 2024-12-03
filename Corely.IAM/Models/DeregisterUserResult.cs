namespace Corely.IAM.Models;

public record DeregisterUserResult(
    bool IsSuccess,
    string? Message)
    : ResultBase(IsSuccess, Message);
