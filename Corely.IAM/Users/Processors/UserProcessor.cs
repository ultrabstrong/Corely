﻿using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Mappers;
using Corely.IAM.Processors;
using Corely.IAM.Security.Enums;
using Corely.IAM.Security.Processors;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Models;
using Corely.IAM.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Corely.IAM.Users.Processors;

internal class UserProcessor : ProcessorBase, IUserProcessor
{
    private readonly IRepo<UserEntity> _userRepo;
    private readonly ISecurityProcessor _securityProcessor;

    public UserProcessor(
        IRepo<UserEntity> userRepo,
        ISecurityProcessor securityProcessor,
        IMapProvider mapProvider,
        IValidationProvider validationProvider,
        ILogger<UserProcessor> logger)
        : base(mapProvider, validationProvider, logger)
    {
        _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
        _securityProcessor = securityProcessor.ThrowIfNull(nameof(securityProcessor));
    }

    public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request)
    {
        return await LogRequestResultAspect(nameof(UserProcessor), nameof(CreateUserAsync), request, async () =>
        {
            var user = MapThenValidateTo<User>(request);

            var existingUser = await _userRepo.GetAsync(u =>
                u.Username == request.Username || u.Email == request.Email);

            if (existingUser != null)
            {
                bool usernameExists = existingUser.Username == request.Username;
                bool emailExists = existingUser.Email == request.Email;

                if (usernameExists)
                    Logger.LogWarning("User already exists with Username {ExistingUsername}", existingUser.Username);
                if (emailExists)
                    Logger.LogWarning("User already exists with Email {ExistingEmail}", existingUser.Email);

                string usernameExistsMessage = usernameExists ? $"Username {request.Username} already exists." : string.Empty;
                string emailExistsMessage = emailExists ? $"Email {request.Email} already exists." : string.Empty;

                return new CreateUserResult(CreateUserResultCode.UserExistsError, $"{usernameExistsMessage} {emailExistsMessage}".Trim(), -1);
            }

            user.SymmetricKeys = [_securityProcessor.GetSymmetricEncryptionKeyEncryptedWithSystemKey()];
            user.AsymmetricKeys = [
                _securityProcessor.GetAsymmetricEncryptionKeyEncryptedWithSystemKey(),
                _securityProcessor.GetAsymmetricSignatureKeyEncryptedWithSystemKey()];

            var userEntity = MapTo<UserEntity>(user)!; // user is validated
            var createdId = await _userRepo.CreateAsync(userEntity);

            return new CreateUserResult(CreateUserResultCode.Success, string.Empty, createdId);
        });
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        return await LogRequestAspect(nameof(UserProcessor), nameof(GetUserAsync), userId, async () =>
        {
            var userEntity = await _userRepo.GetAsync(userId);

            if (userEntity == null)
            {
                Logger.LogInformation("User with Id {UserId} not found", userId);
                return null;
            }

            var user = MapTo<User>(userEntity);
            return user;
        });
    }

    public async Task<User?> GetUserAsync(string userName)
    {
        return await LogRequestAspect(nameof(UserProcessor), nameof(GetUserAsync), userName, async () =>
        {
            var userEntity = await _userRepo.GetAsync(u => u.Username == userName);

            if (userEntity == null)
            {
                Logger.LogInformation("User with Username {Username} not found", userName);
                return null;
            }

            var user = MapTo<User>(userEntity);
            return user;
        });
    }

    public async Task UpdateUserAsync(User user)
    {
        await LogAspect(nameof(UserProcessor), nameof(UpdateUserAsync), async () =>
        {
            Validate(user);
            var userEntity = MapTo<UserEntity>(user)!; // user is validated
            await _userRepo.UpdateAsync(userEntity);
        });
    }

    public async Task<string?> GetUserAuthTokenAsync(int userId)
    {
        return await LogRequestAspect(nameof(UserProcessor), nameof(GetUserAuthTokenAsync), userId, async () =>
        {
            var signatureKey = await GetUserAsymmetricKeyAsync(userId, KeyUsedFor.Signature);
            if (signatureKey == null)
            {
                return null;
            }

            var privateKey = _securityProcessor.DecryptWithSystemKey(signatureKey.EncryptedPrivateKey);
            var credentials = _securityProcessor.GetAsymmetricSigningCredentials(signatureKey.ProviderTypeCode, privateKey, true);

            // Todo - include permission-based scopes & roles

            var token = new JwtSecurityToken(
                issuer: typeof(UserProcessor).FullName,
                claims: [
                    new Claim(JwtRegisteredClaimNames.Sub, "user_id"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ],
                expires: DateTime.Now.Add(TimeSpan.FromSeconds(3600)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        });
    }

    public async Task<bool> IsUserAuthTokenValidAsync(int userId, string authToken)
    {
        return await LogRequestResultAspect(nameof(UserProcessor), nameof(IsUserAuthTokenValidAsync), new { userId, authToken }, async () =>
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(authToken))
            {
                Logger.LogInformation("Auth token is in invalid format");
                return false;
            }

            var signatureKey = await GetUserAsymmetricKeyAsync(userId, KeyUsedFor.Signature);
            if (signatureKey == null)
            {
                return false;
            }

            var credentials = _securityProcessor.GetAsymmetricSigningCredentials(signatureKey.ProviderTypeCode, signatureKey.PublicKey, false);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = credentials.Key,
                ValidateIssuer = true,
                ValidIssuer = typeof(UserProcessor).FullName,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var result = false;
            try
            {
                tokenHandler.ValidateToken(authToken, validationParameters, out _);
                result = true;
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Token validation failed: {Error}", ex.Message);
            }

            return result;
        });
    }

    public async Task<string?> GetAsymmetricSignatureVerificationKeyAsync(int userId)
    {
        return await LogRequestResultAspect(nameof(UserProcessor), nameof(GetAsymmetricSignatureVerificationKeyAsync), userId, async () =>
        {
            var signatureKey = await GetUserAsymmetricKeyAsync(userId, KeyUsedFor.Signature);
            if (signatureKey == null)
            {
                return null;
            }
            return signatureKey.PublicKey;
        });
    }

    private async Task<UserAsymmetricKeyEntity?> GetUserAsymmetricKeyAsync(int userId, KeyUsedFor keyUse)
    {
        var userEntity = await _userRepo.GetAsync(
           u => u.Id == userId,
           include: q => q
               .Include(u => u.AsymmetricKeys));

        if (userEntity == null)
        {
            Logger.LogWarning("User with Id {UserId} not found", userId);
            return null;
        }

        var signatureKey = userEntity.AsymmetricKeys?.FirstOrDefault(k => k.KeyUsedFor == keyUse);
        if (signatureKey == null)
        {
            Logger.LogWarning("User with Id {UserId} does not have an asymmetric signature key", userId);
            return null;
        }

        return signatureKey;
    }
}
