using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Mappers;
using Corely.IAM.Security.Processors;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Processors;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Corely.UnitTests.IAM.Users.Processors;

public class UserProcessorTests
{
    private const string VALID_USERNAME = "username";
    private const string VALID_EMAIL = "email@x.y";

    private readonly Fixture _fixture = new();
    private readonly ServiceFactory _serviceFactory = new();
    private readonly UserProcessor _userProcessor;

    public UserProcessorTests()
    {
        _userProcessor = new UserProcessor(
            _serviceFactory.GetRequiredService<IRepo<UserEntity>>(),
            _serviceFactory.GetRequiredService<ISecurityProcessor>(),
            _serviceFactory.GetRequiredService<IMapProvider>(),
            _serviceFactory.GetRequiredService<IValidationProvider>(),
            _serviceFactory.GetRequiredService<ILogger<UserProcessor>>());
    }

    [Fact]
    public async Task CreateUserAsync_Fails_WhenUserExists()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        await _userProcessor.CreateUserAsync(createUserRequest);

        var createUserResult = await _userProcessor.CreateUserAsync(createUserRequest);

        Assert.Equal(CreateUserResultCode.UserExistsError, createUserResult.ResultCode);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreateUserResult()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var res = await _userProcessor.CreateUserAsync(createUserRequest);
        Assert.Equal(CreateUserResultCode.Success, res.ResultCode);
    }

    [Fact]
    public async Task CreateUser_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _userProcessor.CreateUserAsync(null!));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task GetUserByUseridAsync_ReturnsNull_WhenUserDNE()
    {
        var user = await _userProcessor.GetUserAsync(_fixture.Create<int>());

        Assert.Null(user);
    }

    [Fact]
    public async Task GetUserByUseridAsync_ReturnsUser_WhenUserExists()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);

        var user = await _userProcessor.GetUserAsync(createResult.CreatedId);

        Assert.NotNull(user);
        Assert.Equal(createUserRequest.Username, user.Username);
        Assert.Equal(createUserRequest.Email, user.Email);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ReturnsNull_WhenUserDNE()
    {
        var user = await _userProcessor.GetUserAsync(_fixture.Create<string>());

        Assert.Null(user);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ReturnsUser_WhenUserExists()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        await _userProcessor.CreateUserAsync(createUserRequest);

        var user = await _userProcessor.GetUserAsync(createUserRequest.Username);

        Assert.NotNull(user);
        Assert.Equal(createUserRequest.Username, user.Username);
        Assert.Equal(createUserRequest.Email, user.Email);
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesUser()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        await _userProcessor.CreateUserAsync(createUserRequest);
        var user = await _userProcessor.GetUserAsync(createUserRequest.Username);
        user!.Disabled = false;

        await _userProcessor.UpdateUserAsync(user);
        var updatedUser = await _userProcessor.GetUserAsync(createUserRequest.Username);

        Assert.False(updatedUser!.Disabled);
    }

    [Fact]
    public async Task GetUserAuthTokenAsync_ReturnsAuthToken()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);

        var token = await _userProcessor.GetUserAuthTokenAsync(createResult.CreatedId);

        Assert.NotNull(token);

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        Assert.Equal(typeof(UserProcessor).FullName, jwtToken.Issuer);
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "user_id");
        Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
    }

    [Fact]
    public async Task GetUserAuthTokenAsync_ReturnsNull_WhenUserDNE()
    {
        var token = await _userProcessor.GetUserAuthTokenAsync(_fixture.Create<int>());

        Assert.Null(token);
    }

    [Fact]
    public async Task GetUserAuthTokenAsync_ReturnsNull_WhenSignatureKeyDNE()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);

        var userRepo = _serviceFactory.GetRequiredService<IRepo<UserEntity>>();
        var user = await userRepo.GetAsync(createResult.CreatedId);
        user?.SymmetricKeys?.Clear();
        user?.AsymmetricKeys?.Clear();
        await userRepo.UpdateAsync(user!);

        var token = await _userProcessor.GetUserAuthTokenAsync(createResult.CreatedId);

        Assert.Null(token);
    }

    [Fact]
    public async Task IsUserAuthTokenValidAsync_ReturnsTrue_WithValidToken()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);
        var token = await _userProcessor.GetUserAuthTokenAsync(createResult.CreatedId);

        var isValid = await _userProcessor.IsUserAuthTokenValidAsync(createResult.CreatedId, token!);

        Assert.True(isValid);
    }

    [Fact]
    public async Task IsUserAuthTokenValidAsync_ReturnsFalse_WithInvalidTokenFormat()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);
        var token = await _userProcessor.GetUserAuthTokenAsync(createResult.CreatedId);

        var isValid = await _userProcessor.IsUserAuthTokenValidAsync(createResult.CreatedId, token! + "invalid");

        Assert.False(isValid);
    }

    [Fact]
    public async Task IsUserAuthTokenValidAsync_ReturnsFalse_WhenUserDNE()
    {
        var isValid = await _userProcessor.IsUserAuthTokenValidAsync(_fixture.Create<int>(), _fixture.Create<string>());

        Assert.False(isValid);
    }

    [Fact]
    public async Task IsUserAuthTokenValidAsync_ReturnsFalse_WhenSignatureKeyDNE()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);
        var token = await _userProcessor.GetUserAuthTokenAsync(createResult.CreatedId);

        var userRepo = _serviceFactory.GetRequiredService<IRepo<UserEntity>>();
        var user = await userRepo.GetAsync(createResult.CreatedId);
        user?.SymmetricKeys?.Clear();
        user?.AsymmetricKeys?.Clear();
        await userRepo.UpdateAsync(user!);

        var isValid = await _userProcessor.IsUserAuthTokenValidAsync(createResult.CreatedId, token!);

        Assert.False(isValid);
    }

    [Fact]
    public async Task IsUserAuthTokenValidAsync_ReturnsFalse_WithInvalidToken()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);

        var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken());

        var isValid = await _userProcessor.IsUserAuthTokenValidAsync(createResult.CreatedId, token! + "invalid");

        Assert.False(isValid);
    }

    [Fact]
    public async Task GetAsymmetricSignatureVerificationKeyAsync_ReturnsKey()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);

        var key = await _userProcessor.GetAsymmetricSignatureVerificationKeyAsync(createResult.CreatedId);

        Assert.NotNull(key);
    }

    [Fact]
    public async Task GetAsymmetricSignatureVerificationKeyAsync_ReturnsNull_WhenUserDNE()
    {
        var key = await _userProcessor.GetAsymmetricSignatureVerificationKeyAsync(_fixture.Create<int>());

        Assert.Null(key);
    }

    [Fact]
    public async Task GetAsymmetricSignatureVerificationKeyAsync_ReturnsNull_WhenSignatureKeyDNE()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var createResult = await _userProcessor.CreateUserAsync(createUserRequest);

        var userRepo = _serviceFactory.GetRequiredService<IRepo<UserEntity>>();
        var user = await userRepo.GetAsync(createResult.CreatedId);
        user?.AsymmetricKeys?.Clear();
        await userRepo.UpdateAsync(user!);

        var key = await _userProcessor.GetAsymmetricSignatureVerificationKeyAsync(createResult.CreatedId);

        Assert.Null(key);
    }
}
