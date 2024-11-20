using Corely.Common.Extensions;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.BasicAuths.Entities;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.Enums;
using Corely.IAM.Mappers;
using Corely.IAM.Processors;
using Corely.IAM.Validators;
using Corely.Security.Password;
using Corely.Security.PasswordValidation.Providers;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.BasicAuths.Processors
{
    internal class BasicAuthProcessor : ProcessorBase, IBasicAuthProcessor
    {
        private readonly IRepo<BasicAuthEntity> _basicAuthRepo;
        private readonly IPasswordValidationProvider _passwordValidationProvider;

        public BasicAuthProcessor(
            IRepo<BasicAuthEntity> basicAuthRepo,
            IPasswordValidationProvider passwordValidationProvider,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<BasicAuthProcessor> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _basicAuthRepo = basicAuthRepo.ThrowIfNull(nameof(basicAuthRepo));
            _passwordValidationProvider = passwordValidationProvider.ThrowIfNull(nameof(passwordValidationProvider));
        }

        public async Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request)
        {
            LogRequest(nameof(BasicAuthProcessor), nameof(UpsertBasicAuthAsync), request);

            var basicAuth = MapThenValidateTo<BasicAuth>(request);

            var passwordValidationResults = _passwordValidationProvider.ValidatePassword(request.Password);
            if (!passwordValidationResults.IsSuccess)
            {
                throw new PasswordValidationException(passwordValidationResults, "Password validation failed");
            }

            var basicAuthEntity = MapTo<BasicAuthEntity>(basicAuth);

            var existingAuth = await _basicAuthRepo.GetAsync(e => e.UserId == basicAuthEntity.UserId);

            UpsertBasicAuthResult result = null!;
            if (existingAuth?.Id == null)
            {
                Logger.LogDebug("No existing basic auth for UserId {UserId}. Creating new", request.UserId);
                var newId = await _basicAuthRepo.CreateAsync(basicAuthEntity);
                result = new UpsertBasicAuthResult(true, string.Empty, newId, UpsertType.Create);
            }
            else
            {
                Logger.LogDebug("Found existing basic auth for UserId {UserId}. Updating", request.UserId);
                await _basicAuthRepo.UpdateAsync(basicAuthEntity);
                result = new UpsertBasicAuthResult(true, string.Empty, existingAuth.Id, UpsertType.Update);
            }

            return LogResult(nameof(BasicAuthProcessor), nameof(UpsertBasicAuthAsync), result);
        }

        public async Task<bool> VerifyBasicAuthAsync(VerifyBasicAuthRequest request)
        {
            LogRequest(nameof(BasicAuthProcessor), nameof(VerifyBasicAuthAsync), request);

            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var basicAuthEntity = await _basicAuthRepo.GetAsync(e => e.UserId == request.UserId);

            if (basicAuthEntity == null)
            {
                Logger.LogInformation("No basic auth found for UserId {UserId}", request.UserId);
                return false;
            }

            var basicAuth = MapTo<BasicAuth>(basicAuthEntity);

            var result = basicAuth.Password.Verify(request.Password);

            return LogResult(nameof(BasicAuthProcessor), nameof(VerifyBasicAuthAsync), result);
        }
    }
}
