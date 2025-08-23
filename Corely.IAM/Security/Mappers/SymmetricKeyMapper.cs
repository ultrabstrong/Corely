using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Factories;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Security.Mappers;

internal static class SymmetricKeyMapper
{
    private static ISymmetricEncryptionProviderFactory? _encryptionProviderFactory;

    // This would be set by dependency injection in a real scenario
    internal static void SetEncryptionProviderFactory(ISymmetricEncryptionProviderFactory factory)
    {
        _encryptionProviderFactory = factory;
    }

    public static SymmetricKeyEntity ToEntity(SymmetricKey source)
    {
        if (source == null) return null!;
        
        return new SymmetricKeyEntity
        {
            Id = source.Id,
            EncryptedKey = EncryptKey(source.Key),
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
            Key = DecryptKey(source.EncryptedKey),
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

    private static string EncryptKey(string key)
    {
        if (_encryptionProviderFactory == null)
            return key; // Fallback if not configured

        var provider = _encryptionProviderFactory.GetDefaultProvider();
        return provider.Encrypt(key);
    }

    private static string DecryptKey(string encryptedKey)
    {
        if (_encryptionProviderFactory == null)
            return encryptedKey; // Fallback if not configured

        var provider = _encryptionProviderFactory.GetProviderForDecrypting(encryptedKey);
        return provider.Decrypt(encryptedKey);
    }
}