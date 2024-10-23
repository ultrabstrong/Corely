using AutoMapper;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Mappers.AutoMapper.SecurityProfiles
{
    public class SymmetricKeyProfile : Profile
    {
        public SymmetricKeyProfile()
        {
            CreateMap<SymmetricKey, SymmetricKeyEntity>()
                .ForMember(m => m.EncryptedKey, opt => opt.MapFrom(src => src.Key))
                .ReverseMap();

            CreateMap<SymmetricKey, AccountSymmetricKeyEntity>()
                .IncludeBase<SymmetricKey, SymmetricKeyEntity>()
                .ForMember(m => m.AccountId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<SymmetricKey, UserSymmetricKeyEntity>()
                .IncludeBase<SymmetricKey, SymmetricKeyEntity>()
                .ForMember(m => m.UserId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
