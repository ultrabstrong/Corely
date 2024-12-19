using Corely.IAM.Groups.Enums;

namespace Corely.IAM.Models;

public record RegisterUsersWithGroupResult(
    AddUsersToGroupResultCode ResultCode,
    string Message,
    int RegisteredUserCount,
    List<int> InvalidUserIds = null);
