namespace Corely.IAM.Models
{
    public record RegisterUsersWithGroupResult(
        bool IsSuccess,
        string Message,
        int RegisteredUserCount,
        List<int> InvalidUserIds = null)
        : ResultBase(IsSuccess, Message);
}
