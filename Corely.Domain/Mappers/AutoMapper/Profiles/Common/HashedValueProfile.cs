﻿using AutoMapper;
using Corely.Common.Models.Security;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;

namespace Corely.Domain.Mappers.AutoMapper.Profiles.Common
{
    public class HashedValueProfile : Profile
    {
        public HashedValueProfile()
        {
            CreateMap<IHashedValue, string>().ConvertUsing<HashedValueToStringTypeConverter>();
            CreateMap<string, IHashedValue>().ConvertUsing<StringToHashedValueTypeConverter>();
        }
    }
}
