using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Entities;
using Corely.IAM.Security.Factories;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Security.Mappers;

internal static class AsymmetricKeyMapper
{
    private static ISymmetricEncryptionProviderFactory? _encryptionProviderFactory;

    // This would be set by dependency injection in a real scenario
    internal static void SetEncryptionProviderFactory(ISymmetricEncryptionProviderFactory factory)
    {
        _encryptionProviderFactory = factory;
    }

    public static AsymmetricKeyEntity ToEntity(AsymmetricKey source)
    {
        if (source == null) return null!;
        
        return new AsymmetricKeyEntity
        {
            Id = source.Id,
            PublicKey = source.PublicKey,
            EncryptedPrivateKey = EncryptPrivateKey(source.PrivateKey),
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
            PrivateKey = DecryptPrivateKey(source.EncryptedPrivateKey),
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

    private static string EncryptPrivateKey(string privateKey)
    {
        if (_encryptionProviderFactory == null)
            return privateKey; // Fallback if not configured

        var provider = _encryptionProviderFactory.GetDefaultProvider();
        return provider.Encrypt(privateKey);
    }

    private static string DecryptPrivateKey(string encryptedPrivateKey)
    {
        if (_encryptionProviderFactory == null)
            return encryptedPrivateKey; // Fallback if not configured

        var provider = _encryptionProviderFactory.GetProviderForDecrypting(encryptedPrivateKey);
        return provider.Decrypt(encryptedPrivateKey);
    }
}