﻿using AutoMapper;
using Corely.IAM.Mappers.AutoMapper.TypeConverters;
using Corely.Security.Encryption.Models;

namespace Corely.IAM.Mappers.AutoMapper.SecurityProfiles
{
    internal class EncryptedValueProfile : Profile
    {
        public EncryptedValueProfile()
        {
            CreateMap<ISymmetricEncryptedValue, string?>().ConvertUsing<EncryptedValueToStringTypeConverter>();
            CreateMap<string, ISymmetricEncryptedValue>().ConvertUsing<EncryptedStringToEncryptedValueTypeConverter>();
        }
    }
}
