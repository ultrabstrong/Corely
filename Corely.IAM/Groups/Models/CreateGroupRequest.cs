namespace Corely.IAM.Groups.Models
{
    public record CreateGroupRequest(
        string GroupName,
        int OwnerAccountId);
}
