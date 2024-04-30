using AutoMapper;
using Corely.Common.Extensions;
using Corely.Security.Encryption.Models;
using Corely.Security.Hashing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corely.IAM.Mappers.AutoMapper.ValueConverters
{
    internal sealed class PlainStringToSymmetricEncryptedValueValueProvider : IValueConverter<string, ISymmetricEncryptedValue>
    {
        private readonly ISecurityConfigurationProvider _securityConfigurationProvider;

        public PlainStringToSymmetricEncryptedValueValueProvider(ISecurityConfigurationProvider securityConfigurationProvider)
        {
            _securityConfigurationProvider = securityConfigurationProvider.ThrowIfNull(nameof(securityConfigurationProvider));
            ArgumentNullException.ThrowIfNull(
                securityConfigurationProvider.GetSystemSymmetricKey(),
                nameof(securityConfigurationProvider.GetSystemSymmetricKey));
        }

        public ISymmetricEncryptedValue Convert(string sourceMember, ResolutionContext context)
        {
            var systemKey = _securityConfigurationProvider
                .GetSystemSymmetricKey()
                .GetCurrentVersion()
                .Key;

            // Need to figure out best way to create a symmetricEcryptedValue without coupling this to an implementation
        }
    }
}
