using AutoMapper;
using Corely.IAM.Models.Users;

namespace Corely.IAM.Mappers.AutoMapper.Profiles.Users
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
