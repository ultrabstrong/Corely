namespace Corely.IAM.Groups.Models;

public enum CreateGroupResultCode
{
    GroupExistsError,
    AccountNotFoundError,
    Success
}

internal record CreateGroupResult(
    CreateGroupResultCode ResultCode,
    string Message,
    int CreatedId);
