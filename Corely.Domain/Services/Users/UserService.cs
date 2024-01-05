using Corely.Common.Extensions;
using Corely.Domain.Entities.Users;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Corely.Domain.Models.Users;
using Corely.Domain.Repos;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.Domain.Services.Users
{
    public class UserService : IUserService
    {
        protected readonly IValidationProvider _validationProvider;
        protected readonly IMapProvider _mapProvider;
        protected readonly ILogger _logger;
        private readonly IUserRepo _userRepo;

        public UserService(
            IUserRepo userRepo,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<UserService> logger)
        {
            _validationProvider = validationProvider.ThrowIfNull(nameof(validationProvider));
            _mapProvider = mapProvider.ThrowIfNull(nameof(mapProvider));
            _logger = logger.ThrowIfNull(nameof(logger));
            _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
        }

        public async Task<CreateUserResult> Create(CreateUserRequest createUserRequest)
        {
            try
            {
                _logger.LogInformation("Creating user {Username}", createUserRequest.Username);

                var user = _mapProvider.Map<User>(createUserRequest);
                _validationProvider.ThrowIfInvalid(user);

                var basicauth = _mapProvider.Map<BasicAuth>(createUserRequest);
                _validationProvider.ThrowIfInvalid(basicauth);

                var userEntity = _mapProvider.Map<UserEntity>(user);
                if (await _userRepo.DoesUserExist(userEntity.Username, userEntity.Email))
                {
                    _logger.LogWarning("User with username {Username} or email {Email} already exists",
                        userEntity.Username, userEntity.Email);
                    throw new UserServiceException() { Reason = UserServiceException.ErrorReason.UserAlreadyExists };
                }
                await _userRepo.Create(userEntity);
                _logger.LogInformation("User {Username} created", user.Username);
                return new CreateUserResult();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Model is not valid");
                throw new UserServiceException(ex.Message, ex)
                {
                    Reason = UserServiceException.ErrorReason.ValidationFailed
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {Username}", createUserRequest.Username);
                throw new UserServiceException(ex.Message, ex)
                {
                    Reason = UserServiceException.ErrorReason.Unknown
                };
            }
        }
    }
}
