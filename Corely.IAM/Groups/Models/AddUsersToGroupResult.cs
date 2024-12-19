using Corely.IAM.Groups.Enums;

namespace Corely.IAM.Groups.Models;

internal record AddUsersToGroupResult(
    AddUsersToGroupResultCode ResultCode,
    string? Message,
    int AddedUserCount,
    List<int> InvalidUserIds = null);

