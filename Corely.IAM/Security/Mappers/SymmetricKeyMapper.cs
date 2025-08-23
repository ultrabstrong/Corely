using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Security.Mappers;

internal static class SymmetricKeyMapper
{
    public static SymmetricKeyEntity ToEntity(SymmetricKey source)
    {
        if (source == null) return null!;
        
        return new SymmetricKeyEntity
        {
            Id = source.Id,
            EncryptedKey = source.Key, // In real implementation, this would be encrypted
            CreatedAt = source.CreatedAt,
            ExpiresAt = source.ExpiresAt
        };
    }

    public static SymmetricKey ToModel(SymmetricKeyEntity source)
    {
        if (source == null) return null!;
        
        return new SymmetricKey
        {
            Id = source.Id,
            Key = source.EncryptedKey, // In real implementation, this would be decrypted
            CreatedAt = source.CreatedAt,
            ExpiresAt = source.ExpiresAt
        };
    }

    public static AccountSymmetricKeyEntity ToAccountEntity(SymmetricKey source)
    {
        if (source == null) return null!;
        
        var baseEntity = ToEntity(source);
        return new AccountSymmetricKeyEntity
        {
            Id = baseEntity.Id,
            EncryptedKey = baseEntity.EncryptedKey,
            CreatedAt = baseEntity.CreatedAt,
            ExpiresAt = baseEntity.ExpiresAt
            // AccountId is ignored per original mapping
        };
    }

    public static SymmetricKey ToModel(AccountSymmetricKeyEntity source)
    {
        if (source == null) return null!;
        
        return ToModel((SymmetricKeyEntity)source);
    }

    public static UserSymmetricKeyEntity ToUserEntity(SymmetricKey source)
    {
        if (source == null) return null!;
        
        var baseEntity = ToEntity(source);
        return new UserSymmetricKeyEntity
        {
            Id = baseEntity.Id,
            EncryptedKey = baseEntity.EncryptedKey,
            CreatedAt = baseEntity.CreatedAt,
            ExpiresAt = baseEntity.ExpiresAt
            // UserId is ignored per original mapping
        };
    }

    public static SymmetricKey ToModel(UserSymmetricKeyEntity source)
    {
        if (source == null) return null!;
        
        return ToModel((SymmetricKeyEntity)source);
    }
}