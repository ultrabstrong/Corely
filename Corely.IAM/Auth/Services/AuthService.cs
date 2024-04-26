﻿using Corely.Common.Extensions;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Auth.Models;
using Corely.IAM.Enums;
using Corely.IAM.Mappers;
using Corely.IAM.Repos;
using Corely.IAM.Services;
using Corely.IAM.Validators;
using Corely.Security.Password;
using Corely.Security.PasswordValidation.Providers;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Auth.Services
{
    internal class AuthService : ServiceBase, IAuthService
    {
        private readonly IRepoExtendedGet<BasicAuthEntity> _basicAuthRepo;
        private readonly IPasswordValidationProvider _passwordValidationProvider;

        public AuthService(
            IRepoExtendedGet<BasicAuthEntity> basicAuthRepo,
            IPasswordValidationProvider passwordValidationProvider,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<AuthService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _basicAuthRepo = basicAuthRepo.ThrowIfNull(nameof(basicAuthRepo));
            _passwordValidationProvider = passwordValidationProvider.ThrowIfNull(nameof(passwordValidationProvider));
        }

        public async Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var passwordValidationResults = _passwordValidationProvider.ValidatePassword(request.Password);
            if (!passwordValidationResults.IsSuccess)
            {
                throw new PasswordValidationException(passwordValidationResults, "Password validation failed");
            }

            var basicAuth = MapAndValidate<BasicAuth>(request);

            var basicAuthEntity = Map<BasicAuthEntity>(basicAuth);

            var existingAuth = await _basicAuthRepo.GetAsync(e => e.UserId == basicAuthEntity.UserId);

            UpsertBasicAuthResult result = null!;
            if (existingAuth?.Id == null)
            {
                Logger.LogDebug("No existing basic auth for UserId {UserId}. Creating new", request.UserId);
                var newId = await _basicAuthRepo.CreateAsync(basicAuthEntity);
                result = new UpsertBasicAuthResult(true, "", newId, UpsertType.Create);
            }
            else
            {
                Logger.LogDebug("Found existing basic auth for UserId {UserId}. Updating", request.UserId);
                await _basicAuthRepo.UpdateAsync(basicAuthEntity);
                result = new UpsertBasicAuthResult(true, "", existingAuth.Id, UpsertType.Update);
            }

            Logger.LogInformation("Upserted basic auth for UserId {UserId}", request.UserId);
            return result;
        }
    }
}
