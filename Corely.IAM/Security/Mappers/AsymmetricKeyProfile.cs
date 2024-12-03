using AutoMapper;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Security.Mappers;

internal class AsymmetricKeyProfile : Profile
{
    public AsymmetricKeyProfile()
    {
        CreateMap<AsymmetricKey, AsymmetricKeyEntity>()
            .ForMember(m => m.EncryptedPrivateKey, opt => opt.MapFrom(src => src.PrivateKey))
            .ReverseMap();

        CreateMap<AsymmetricKey, AccountAsymmetricKeyEntity>()
            .IncludeBase<AsymmetricKey, AsymmetricKeyEntity>()
            .ForMember(m => m.AccountId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<AsymmetricKey, UserAsymmetricKeyEntity>()
            .IncludeBase<AsymmetricKey, AsymmetricKeyEntity>()
            .ForMember(m => m.UserId, opt => opt.Ignore())
            .ReverseMap();
    }
}
