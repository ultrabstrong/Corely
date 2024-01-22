using Corely.Common.Extensions;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Enums;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Corely.Domain.Repos;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.Domain.Services.Auth
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly IRepoExtendedGet<BasicAuthEntity> _basicAuthRepo;

        public AuthService(
            IRepoExtendedGet<BasicAuthEntity> basicAuthRepo,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<AuthService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _basicAuthRepo = basicAuthRepo.ThrowIfNull(nameof(basicAuthRepo));
        }

        public async Task<UpsertBasicAuthResult> UpsertBasicAuthAsync(UpsertBasicAuthRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var basicAuth = MapToValid<BasicAuth>(request);

            var basicAuthEntity = mapProvider.Map<BasicAuthEntity>(basicAuth);

            var existingAuth = await _basicAuthRepo.GetAsync(e => e.UserId == basicAuthEntity.UserId);

            UpsertBasicAuthResult result = null!;
            if (existingAuth?.Id == null)
            {
                logger.LogDebug("No existing basic auth for UserId {UserId}. Creating new", request.UserId);
                var newId = await _basicAuthRepo.CreateAsync(basicAuthEntity);
                result = new UpsertBasicAuthResult(true, "", newId, UpsertType.Create);
            }
            else
            {
                logger.LogDebug("Found existing basic auth for UserId {UserId}. Updating", request.UserId);
                await _basicAuthRepo.UpdateAsync(basicAuthEntity);
                result = new UpsertBasicAuthResult(true, "", existingAuth.Id, UpsertType.Update);
            }

            logger.LogInformation("Upserted basic auth for UserId {UserId}", request.UserId);
            return result;
        }
    }
}
