using AutoMapper;
using Corely.IAM.Users.Models;

namespace Corely.IAM.Users.Mappers;

internal class CreateUserRequestProfile : Profile
{
    public CreateUserRequestProfile()
    {
        CreateMap<CreateUserRequest, User>(MemberList.Source);
    }
}
