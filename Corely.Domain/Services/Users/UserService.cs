using Corely.Common.Extensions;
using Corely.Domain.Entities.Users;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Users;
using Corely.Domain.Repos;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.Domain.Services.Users
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(
            IUserRepo userRepo,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<UserService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
        }

        public async Task<CreateUserResult> CreateUser(CreateUserRequest createUserRequest)
        {
            logger.LogInformation("Creating user {Username}", createUserRequest.Username);

            var user = MapToValid<User>(createUserRequest);
            //var basicAuth = MapToValid<BasicAuth>(createUserRequest);

            var userEntity = mapProvider.Map<UserEntity>(user);

            if (await _userRepo.DoesUserExist(userEntity.Username, userEntity.Email))
            {
                logger.LogWarning("User with username {Username} or email {Email} already exists",
                    userEntity.Username, userEntity.Email);
                throw new UserServiceException() { Reason = UserServiceException.ErrorReason.UserAlreadyExists };
            }

            await _userRepo.Create(userEntity);
            logger.LogInformation("User {Username} created", user.Username);
            return new CreateUserResult();
        }
    }
}
