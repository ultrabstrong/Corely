﻿using Corely.IAM.Entities;
using Corely.IAM.Security.Enums;

namespace Corely.IAM.Security.Entities
{
    public class AsymmetricKeyEntity : IHasCreatedUtc, IHasModifiedUtc
    {
        public KeyUsedFor KeyUsedFor { get; set; }
        public string ProviderTypeCode { get; set; } = null!;
        public int Version { get; set; }
        public string PublicKey { get; set; } = null!;
        public string EncryptedPrivateKey { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
