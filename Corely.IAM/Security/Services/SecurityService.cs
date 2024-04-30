using Corely.Common.Extensions;
using Corely.IAM.Security.Models;
using Corely.Security.Keys.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corely.IAM.Security.Services
{
    internal class SecurityService : ISecurityService
    {
        private readonly ISecurityConfigurationProvider _securityConfigurationProvider;
        private readonly ISymmetricKeyProvider _symmetricKeyProvider;

        public SecurityService(
            ISymmetricKeyProvider symmetricKeyProvider,
            ISecurityConfigurationProvider securityConfigurationProvider)
        { 
            _symmetricKeyProvider = symmetricKeyProvider.ThrowIfNull(nameof(symmetricKeyProvider));
            _securityConfigurationProvider = securityConfigurationProvider.ThrowIfNull(nameof(securityConfigurationProvider));
            ArgumentNullException.ThrowIfNull(
                securityConfigurationProvider.GetSystemSymmetricKey(),
                nameof(securityConfigurationProvider.GetSystemSymmetricKey));
        }

        public SymmetricKey GetSymmetricKeyEncryptedWithSystemKey()
        {
            var newkey = _symmetricKeyProvider.CreateKey();
            var systemKey = _securityConfigurationProvider
                .GetSystemSymmetricKey()
                .GetCurrentVersion()
                .Key;
            return null;
        }
    }
}
