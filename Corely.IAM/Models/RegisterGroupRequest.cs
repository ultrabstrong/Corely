namespace Corely.IAM.Models;

public record RegisterGroupRequest(
    string GroupName,
    int OwnerAccountId,
    List<int>? UserIdsToAdd = null);
