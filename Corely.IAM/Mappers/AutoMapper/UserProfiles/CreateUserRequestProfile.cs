using AutoMapper;
using Corely.IAM.Users.Models;

namespace Corely.IAM.Mappers.AutoMapper.UserProfiles
{
    internal class CreateUserRequestProfile : Profile
    {
        public CreateUserRequestProfile()
        {
            CreateMap<CreateUserRequest, User>(MemberList.Source)
                .ForSourceMember(m => m.AccountId, opt => opt.DoNotValidate());
        }
    }
}
