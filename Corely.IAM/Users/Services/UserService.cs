using Corely.Common.Extensions;
using Corely.IAM.Mappers;
using Corely.IAM.Models;
using Corely.IAM.Repos;
using Corely.IAM.Security.Services;
using Corely.IAM.Services;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Exceptions;
using Corely.IAM.Users.Models;
using Corely.IAM.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Corely.IAM.Users.Services
{
    internal class UserService : ServiceBase, IUserService
    {
        private readonly IRepoExtendedGet<UserEntity> _userRepo;
        private readonly ISecurityService _securityService;

        public UserService(
            IRepoExtendedGet<UserEntity> userRepo,
            ISecurityService securityService,
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger<UserService> logger)
            : base(mapProvider, validationProvider, logger)
        {
            _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
            _securityService = securityService.ThrowIfNull(nameof(securityService));
        }

        public async Task<CreateResult> CreateUserAsync(CreateUserRequest request)
        {
            var user = MapThenValidateTo<User>(request);

            Logger.LogInformation("Creating user {Username}", request.Username);

            await ThrowIfUserExists(user.Username, user.Email);

            user.SymmetricKeys = [_securityService.GetSymmetricEncryptionKeyEncryptedWithSystemKey()];
            user.AsymmetricKeys = [
                _securityService.GetAsymmetricEncryptionKeyEncryptedWithSystemKey(),
                _securityService.GetAsymmetricSignatureKeyEncryptedWithSystemKey()];

            var userEntity = MapTo<UserEntity>(user);
            var createdId = await _userRepo.CreateAsync(userEntity);

            Logger.LogInformation("User {Username} created with Id {Id}", user.Username, createdId);
            return new CreateResult(true, string.Empty, createdId);
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

                string usernameExistsMessage = usernameExists ? $"Username {username} already exists." : string.Empty;
                string emailExistsMessage = emailExists ? $"Email {email} already exists." : string.Empty;

                throw new UserExistsException($"{usernameExistsMessage} {emailExistsMessage}".Trim())
                {
                    UsernameExists = usernameExists,
                    EmailExists = emailExists
                };
            }
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            var userEntity = await _userRepo.GetAsync(userId);

            if (userEntity == null)
            {
                Logger.LogInformation("User with Id {UserId} not found", userId);
                return null;
            }

            return MapTo<User>(userEntity);
        }

        public async Task<User?> GetUserAsync(string userName)
        {
            var userEntity = await _userRepo.GetAsync(u => u.Username == userName);

            if (userEntity == null)
            {
                Logger.LogInformation("User with Username {Username} not found", userName);
                return null;
            }

            return MapTo<User>(userEntity);
        }

        public Task UpdateUserAsync(User user)
        {
            Validate(user);
            var userEntity = MapTo<UserEntity>(user);

            return _userRepo.UpdateAsync(userEntity);
        }

        public async Task<string?> GetUserAuthTokenAsync(int userId)
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

            if (userEntity.AsymmetricKeys == null)
            {
                Logger.LogWarning("User with Id {UserId} does not have an asymmetric key", userId);
                return null;
            }

            // Todo - use our own RSA key pair
            var credentials = new SigningCredentials(
                new RsaSecurityKey(RSA.Create()),
                SecurityAlgorithms.RsaSha256);

            // Todo - include permission-based scopes & roles

            var token = new JwtSecurityToken(
                claims: [
                    new Claim(JwtRegisteredClaimNames.Sub, "user_id"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ],
                expires: DateTime.Now.Add(TimeSpan.FromSeconds(3600)),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
