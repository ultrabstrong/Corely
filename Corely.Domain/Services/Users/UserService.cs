﻿using Corely.Common.Extensions;
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
    internal class UserService(
        IUserRepo userRepo,
        IValidationProvider validationProvider,
        IMapProvider mapProvider,
        ILogger<UserService> logger)
        : IUserService
    {
        private readonly IUserRepo _userRepo = userRepo.ThrowIfNull(nameof(userRepo));
        private readonly IValidationProvider _validationProvider = validationProvider.ThrowIfNull(nameof(validationProvider));
        private readonly IMapProvider _mapProvider = mapProvider;
        private readonly ILogger<UserService> _logger = logger;

        public void Create(User user, BasicAuth basicAuth)
        {
            _logger.LogInformation("Creating user {Username}", user.Username);
            try
            {
                _validationProvider.Validate(user).ThrowIfInvalid();
                _validationProvider.Validate(basicAuth).ThrowIfInvalid();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("User creation args are not valid");
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
                    _logger.LogWarning("User with username {Username} or email {Email} already exists",
                        userEntity.Username, userEntity.Email);
                    throw new UserServiceException() { Reason = UserServiceException.ErrorReason.UserAlreadyExists };
                }
                _userRepo.Create(userEntity);
                _logger.LogInformation("User {Username} created", user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {Username}", user.Username);
                throw new UserServiceException(ex.Message, ex)
                {
                    Reason = UserServiceException.ErrorReason.Unknown
                };
            }
        }
    }
}
