using System.ComponentModel.DataAnnotations;
using AutoFixture;
using Corely.IAM.Mappers;
using Corely.IAM.Processors;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Requests;
using Corely.IAM.Users.Results;
using Corely.IAM.Validation;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.UnitTests.Processors;

public class ProcessorBaseTests
{
    private const string VALID_USERNAME = "testuser";
    private const string VALID_EMAIL = "test@example.com";
    
    private readonly Fixture _fixture = new();
    private readonly ServiceFactory _serviceFactory = new();
    private readonly MockProcessorBase _mockProcessorBase;

    public ProcessorBaseTests()
    {
        _mockProcessorBase = new MockProcessorBase(
            _serviceFactory.GetRequiredService<IMapProvider>(),
            _serviceFactory.GetRequiredService<IValidationProvider>(),
            _serviceFactory.GetRequiredService<ILogger<ProcessorBaseTests>>());
    }

    [Fact]
    public void MapThenValidateTo_ReturnsValidDestination_IfSourceIsValid()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var user = _mockProcessorBase.MapThenValidateTo<User>(createUserRequest);

        Assert.NotNull(user);
        Assert.Equal(createUserRequest.Username, user.Username);
        Assert.Equal(createUserRequest.Email, user.Email);
    }

    [Fact]
    public void MapThenValidateTo_Throws_WhenSourceIsNull()
    {
        var ex = Record.Exception(() => _mockProcessorBase.MapThenValidateTo<object>(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void MapThenValidateTo_Throws_IfDestinationIsUnmapped()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        var ex = Record.Exception(() => _mockProcessorBase.MapThenValidateTo<CreateUserResult>(createUserRequest));

        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
    public void MapTo_ReturnsValidDestination_IfSourceIsValid()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var user = _mockProcessorBase.MapTo<User>(createUserRequest);

        Assert.NotNull(user);
        Assert.Equal(createUserRequest.Username, user.Username);
        Assert.Equal(createUserRequest.Email, user.Email);
    }

    [Fact]
    public void MapTo_ReturnsNull_WhenSourceIsNull()
    {
        var destination = _mockProcessorBase.MapTo<object>(null);
        Assert.Null(destination);
    }

    [Fact]
    public void MapTo_Throws_IfDestinationIsInvalid()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        var ex = Record.Exception(() => _mockProcessorBase.MapTo<CreateUserResult>(createUserRequest));

        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }

    private class MockProcessorBase : ProcessorBase
    {
        public MockProcessorBase(
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger logger)
            : base(mapProvider, validationProvider, logger)
        {
        }

        // Expose protected methods for testing
        public new TDestination? MapTo<TDestination>(object? source) => base.MapTo<TDestination>(source);
        public new TDestination MapThenValidateTo<TDestination>(object source) => base.MapThenValidateTo<TDestination>(source);
    }
}