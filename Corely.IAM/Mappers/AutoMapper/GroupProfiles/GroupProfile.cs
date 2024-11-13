using AutoMapper;
using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Models;

namespace Corely.IAM.Mappers.AutoMapper.GroupProfiles
{
    internal class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupEntity>(MemberList.Source)
                .ReverseMap();
        }
    }
}
