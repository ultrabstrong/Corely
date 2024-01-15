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
        private readonly IRepoExtendedGet<UserEntity> _userRepo;

        public UserService(
            IRepoExtendedGet<UserEntity> userRepo,
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
            await ThrowIfUserExists(user.Username, user.Email);

            var userEntity = mapProvider.Map<UserEntity>(user);
            await _userRepo.CreateAsync(userEntity);

            logger.LogInformation("User {Username} created", user.Username);
            return new CreateUserResult(true, "");
        }

        private async Task ThrowIfUserExists(string username, string email)
        {
            var existingUser = await _userRepo.GetAsync(u =>
                u.Username == username || u.Email == email);
            if (existingUser != null)
            {
                bool usernameExists = existingUser.Username == username;
                bool emailExists = existingUser.Email == email;

                logger.LogWarning("User already exists. {MatchedOnUsername} {MatchedOnEmail}",
                    usernameExists ? $"Matched username {existingUser.Username}." : "",
                    emailExists ? $"Matched email {existingUser.Email}." : "");

                throw new UserExistsException()
                {
                    UsernameExists = usernameExists,
                    EmailExists = emailExists
                };
            }
        }
    }
}
