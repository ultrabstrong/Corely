using Corely.IAM.Groups.Models;
using Corely.IAM.Models;

namespace Corely.IAM.Groups.Services
{
    internal interface IGroupService
    {
        Task<CreateResult> CreateGroupAsync(CreateGroupRequest createGroupRequest);
    }
}
