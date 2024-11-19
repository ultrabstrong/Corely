using Corely.IAM.Groups.Models;
using Corely.IAM.Models;

namespace Corely.IAM.Groups.Processors
{
    internal interface IGroupProcessor
    {
        Task<CreateResult> CreateGroupAsync(CreateGroupRequest createGroupRequest);
        Task<AddUsersToGroupResult> AddUsersToGroupAsync(AddUsersToGroupRequest addUsersToGroupRequest);
    }
}
