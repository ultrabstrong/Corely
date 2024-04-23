using AutoMapper;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;
using Corely.Security.Hashing.Models;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Common
{
    internal sealed class HashedValueProfile : Profile
    {
        public HashedValueProfile()
        {
            CreateMap<IHashedValue, string?>().ConvertUsing<HashedValueToStringTypeConverter>();
            CreateMap<string, IHashedValue>().ConvertUsing<HashStringToHashedValueTypeConverter>();
        }
    }
}
