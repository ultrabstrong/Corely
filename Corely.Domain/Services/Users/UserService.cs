using Corely.Domain.Entities.Users;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Corely.Domain.Models.Users;
using Corely.Domain.Repos;
using Corely.Domain.Validators;
using Corely.Shared.Extensions;
using Serilog;

namespace Corely.Domain.Services.Users
{
    internal class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IValidationProvider _validationProvider;
        private readonly IMapProvider _mapProvider;
        private readonly ILogger _logger;

        public UserService(IUserRepo userRepo,
            IValidationProvider validationProvider,
            IMapProvider mapProvider,
            ILogger logger)
        {
            _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
            _validationProvider = validationProvider.ThrowIfNull(nameof(validationProvider));
            _mapProvider = mapProvider;
            _logger = logger;
        }

        public void Create(User user, BasicAuth basicAuth)
        {
            _logger.Information("Creating user {Username}", user.Username);
            try
            {
                _validationProvider.Validate(user).ThrowIfInvalid();
                _validationProvider.Validate(basicAuth).ThrowIfInvalid();
            }
            catch (ValidationException ex)
            {
                _logger.Warning("User creation args are not valid");
                throw new UserServiceException(ex.Message, ex)
                {
                    Reason = UserServiceException.ErrorReason.ValidationFailed
                };
            }

            try
            {
                var userEntity = _mapProvider.Map<UserEntity>(user);
                if (_userRepo.DoesUserExist(userEntity.Username, userEntity.Email))
                {
                    _logger.Warning("User with username {Username} or email {Email} already exists",
                        userEntity.Username, userEntity.Email);
                    throw new UserServiceException() { Reason = UserServiceException.ErrorReason.UserAlreadyExists };
                }
                _userRepo.Create(userEntity);
                _logger.Information("User {Username} created", user.Username);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating user {Username}", user.Username);
                throw new UserServiceException(ex.Message, ex)
                {
                    Reason = UserServiceException.ErrorReason.Unknown
                };
            }
        }
    }
}
