using Corely.IAM.Models;

namespace Corely.IAM.Groups.Models
{
    internal record AddUsersToGroupResult(
        bool IsSuccess,
        string? Message,
        int RegisteredUserCount,
        List<int> InvalidUserIds = null)
        : ResultBase(IsSuccess, Message);
}
