﻿using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.BasicAuths.Entities;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.BasicAuths.Services;
using Corely.IAM.Enums;
using Corely.IAM.Mappers;
using Corely.IAM.Validators;
using Corely.Security.Password;
using Corely.Security.PasswordValidation.Providers;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.BasicAuths.Services
{
    public class AuthServiceTests
    {
        private const string VALID_PASSWORD = "Password1!";

        private readonly ServiceFactory _serviceFactory = new();
        private readonly BasicAuthService _authService;

        public AuthServiceTests()
        {
            _authService = new BasicAuthService(
                _serviceFactory.GetRequiredService<IRepo<BasicAuthEntity>>(),
                _serviceFactory.GetRequiredService<IPasswordValidationProvider>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<BasicAuthService>>());
        }

        [Fact]
        public async Task UpsertBasicAuthAsync_ReturnsCreateResult_WhenBasicAuthDoesNotExist()
        {
            var request = new UpsertBasicAuthRequest(1, VALID_PASSWORD);
            var result = await _authService.UpsertBasicAuthAsync(request);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(UpsertType.Create, result.UpsertType);
        }

        [Fact]
        public async Task UpsertBasicAuthAsync_ReturnsUpdateResult_WhenBasicAuthExists()
        {
            var request = new UpsertBasicAuthRequest(1, VALID_PASSWORD);
            await _authService.UpsertBasicAuthAsync(request);
            var result = await _authService.UpsertBasicAuthAsync(request);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(UpsertType.Update, result.UpsertType);
        }

        [Fact]
        public async Task UpsertBasicAuthAsync_Throws_WhenPasswordValidationFails()
        {
            var request = new UpsertBasicAuthRequest(1, "password");

            var ex = await Record.ExceptionAsync(() => _authService.UpsertBasicAuthAsync(request));

            Assert.NotNull(ex);
            var pvex = Assert.IsType<PasswordValidationException>(ex);
            Assert.NotNull(pvex.PasswordValidationResult);
            Assert.False(pvex.PasswordValidationResult.IsSuccess);
            Assert.NotEmpty(pvex.PasswordValidationResult.ValidationFailures);
        }


        [Fact]
        public async Task UpsertBasicAuthAsync_Throws_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _authService.UpsertBasicAuthAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task VerifyBasicAuthAsync_ReturnsTrue_WhenBasicAuthExists()
        {
            var request = new UpsertBasicAuthRequest(1, VALID_PASSWORD);
            await _authService.UpsertBasicAuthAsync(request);

            var verifyRequest = new VerifyBasicAuthRequest(1, VALID_PASSWORD);
            var result = await _authService.VerifyBasicAuthAsync(verifyRequest);

            Assert.True(result);
        }

        [Fact]
        public async Task VerifyBasicAuthAsync_ReturnsFalse_WhenPasswordIsIncorrect()
        {
            var request = new UpsertBasicAuthRequest(1, VALID_PASSWORD);
            await _authService.UpsertBasicAuthAsync(request);

            var verifyRequest = new VerifyBasicAuthRequest(1, "password");
            var result = await _authService.VerifyBasicAuthAsync(verifyRequest);

            Assert.False(result);
        }

        [Fact]
        public async Task VerifyBasicAuthAsync_ReturnsFalse_WhenBasicAuthDoesNotExist()
        {
            var request = new VerifyBasicAuthRequest(1, VALID_PASSWORD);

            var result = await _authService.VerifyBasicAuthAsync(request);

            Assert.False(result);
        }

        [Fact]
        public async Task VerifyBasicAuthAsync_Throws_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _authService.VerifyBasicAuthAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

    }
}
