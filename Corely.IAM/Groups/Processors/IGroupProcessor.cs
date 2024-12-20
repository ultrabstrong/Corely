using Corely.IAM.Groups.Models;

namespace Corely.IAM.Groups.Processors;

internal interface IGroupProcessor
{
    Task<CreateGroupResult> CreateGroupAsync(CreateGroupRequest createGroupRequest);
    Task<AddUsersToGroupResult> AddUsersToGroupAsync(AddUsersToGroupRequest addUsersToGroupRequest);
}
