using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Security.Mappers;

internal static class AsymmetricKeyMapper
{
    public static AsymmetricKeyEntity ToEntity(AsymmetricKey source)
    {
        if (source == null) return null!;
        
        return new AsymmetricKeyEntity
        {
            Id = source.Id,
            PublicKey = source.PublicKey,
            EncryptedPrivateKey = source.PrivateKey, // In real implementation, this would be encrypted
            CreatedAt = source.CreatedAt,
            ExpiresAt = source.ExpiresAt
        };
    }

    public static AsymmetricKey ToModel(AsymmetricKeyEntity source)
    {
        if (source == null) return null!;
        
        return new AsymmetricKey
        {
            Id = source.Id,
            PublicKey = source.PublicKey,
            PrivateKey = source.EncryptedPrivateKey, // In real implementation, this would be decrypted
            CreatedAt = source.CreatedAt,
            ExpiresAt = source.ExpiresAt
        };
    }

    public static AccountAsymmetricKeyEntity ToAccountEntity(AsymmetricKey source)
    {
        if (source == null) return null!;
        
        var baseEntity = ToEntity(source);
        return new AccountAsymmetricKeyEntity
        {
            Id = baseEntity.Id,
            PublicKey = baseEntity.PublicKey,
            EncryptedPrivateKey = baseEntity.EncryptedPrivateKey,
            CreatedAt = baseEntity.CreatedAt,
            ExpiresAt = baseEntity.ExpiresAt
            // AccountId is ignored per original mapping
        };
    }

    public static AsymmetricKey ToModel(AccountAsymmetricKeyEntity source)
    {
        if (source == null) return null!;
        
        return ToModel((AsymmetricKeyEntity)source);
    }

    public static UserAsymmetricKeyEntity ToUserEntity(AsymmetricKey source)
    {
        if (source == null) return null!;
        
        var baseEntity = ToEntity(source);
        return new UserAsymmetricKeyEntity
        {
            Id = baseEntity.Id,
            PublicKey = baseEntity.PublicKey,
            EncryptedPrivateKey = baseEntity.EncryptedPrivateKey,
            CreatedAt = baseEntity.CreatedAt,
            ExpiresAt = baseEntity.ExpiresAt
            // UserId is ignored per original mapping
        };
    }

    public static AsymmetricKey ToModel(UserAsymmetricKeyEntity source)
    {
        if (source == null) return null!;
        
        return ToModel((AsymmetricKeyEntity)source);
    }
}