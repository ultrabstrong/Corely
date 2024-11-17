using AutoMapper;
using Corely.IAM.Accounts.Models;

namespace Corely.IAM.Accounts.Mappers
{
    internal class CreateAccountRequestProfile : Profile
    {
        public CreateAccountRequestProfile()
        {
            CreateMap<CreateAccountRequest, Account>(MemberList.Source)
                .ForSourceMember(m => m.OwnerUserId, opt => opt.DoNotValidate());
        }
    }
}
