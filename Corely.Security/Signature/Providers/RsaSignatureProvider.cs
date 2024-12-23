﻿using Corely.Security.Keys;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Corely.Security.Signature.Providers;

public sealed class RsaSignatureProvider : AsymmetricSignatureProviderBase
{
    public override string SignatureTypeCode => AsymmetricSignatureConstants.RSA_SHA256_CODE;

    private readonly RsaKeyProvider _rsaKeyProvider = new();
    private readonly HashAlgorithmName _hashAlgorithm;

    public RsaSignatureProvider(HashAlgorithmName hashAlgorithm)
    {
        _hashAlgorithm = hashAlgorithm;
    }

    protected override string SignInternal(string value, string privateKey)
    {
        var privateKeyBytes = Convert.FromBase64String(privateKey);
        var dataToSign = Encoding.UTF8.GetBytes(value);

        using (var rsa = RSA.Create())
        {
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            var signedBytes = rsa.SignData(dataToSign, _hashAlgorithm, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signedBytes);
        }
    }

    protected override bool VerifyInternal(string value, string signature, string publicKey)
    {
        var publicKeyBytes = Convert.FromBase64String(publicKey);
        var dataToVerify = Encoding.UTF8.GetBytes(value);
        var signatureBytes = Convert.FromBase64String(signature);

        using (var rsa = RSA.Create())
        {
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
            return rsa.VerifyData(dataToVerify, signatureBytes, _hashAlgorithm, RSASignaturePadding.Pkcs1);
        }
    }

    public override SigningCredentials GetSigningCredentials(string key, bool isKeyPrivate)
    {
        var rsa = RSA.Create();
        if (isKeyPrivate)
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(key), out _);
        else
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(key), out _);
        return new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    }

    public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => _rsaKeyProvider;
}
