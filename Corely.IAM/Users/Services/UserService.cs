using Corely.Common.Extensions;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Repos;
using Corely.IAM.Services;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Exceptions;
using Corely.IAM.Users.Models;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Users.Services
{
    internal class UserService : ServiceBase, IUserService
    {
        private readonly IRepoExtendedGet<UserEntity> _userRepo;
        private readonly IReadonlyRepo<AccountEntity> _readonlyAccountRepo;

        public UserService(
            IRepoExtendedGet<UserEntity> userRepo,
            IReadonlyRepo<AccountEntity> readonlyAccountRepo,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<UserService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
            _readonlyAccountRepo = readonlyAccountRepo.ThrowIfNull(nameof(readonlyAccountRepo));
        }

        public async Task<CreateResult> CreateUserAsync(CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Logger.LogInformation("Creating user {Username}", request.Username);

            var user = MapAndValidate<User>(request);
            await ThrowIfUserExists(user.Username, user.Email);

            var accountEntity = await _readonlyAccountRepo.GetAsync(request.AccountId);
            if (accountEntity == null)
            {
                Logger.LogWarning("Account with Id {AccountId} not found", request.AccountId);
                throw new AccountDoesNotExistException($"Account with Id {request.AccountId} not found");
            }

            var userEntity = Map<UserEntity>(user);
            userEntity.Accounts = [accountEntity];
            var createdId = await _userRepo.CreateAsync(userEntity);

            Logger.LogInformation("User {Username} created with Id {Id}", user.Username, createdId);
            return new CreateResult(true, "", createdId);
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            var userEntity = await _userRepo.GetAsync(userId);
            return MapOrNull<User>(userEntity);
        }

        private async Task ThrowIfUserExists(string username, string email)
        {
            var existingUser = await _userRepo.GetAsync(u =>
                u.Username == username || u.Email == email);
            if (existingUser != null)
            {
                bool usernameExists = existingUser.Username == username;
                bool emailExists = existingUser.Email == email;

                if (usernameExists)
                    Logger.LogWarning("User already exists with Username {ExistingUsername}", existingUser.Username);
                if (emailExists)
                    Logger.LogWarning("User already exists with Email {ExistingEmail}", existingUser.Email);

                string usernameExistsMessage = usernameExists ? $"Username {username} already exists." : "";
                string emailExistsMessage = emailExists ? $"Email {email} already exists." : "";

                throw new UserExistsException($"{usernameExistsMessage} {emailExistsMessage}".Trim())
                {
                    UsernameExists = usernameExists,
                    EmailExists = emailExists
                };
            }
        }
    }
}
