﻿using AutoFixture;
using AutoMapper;
using Corely.IAM.Mappers;
using Corely.IAM.Processors;
using Corely.IAM.Users.Models;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Processors;

public class ProcessorBaseTests
{
    private class MockProcessorBase : ProcessorBase
    {
        public MockProcessorBase(
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger logger)
            : base(mapProvider, validationProvider, logger)
        {
        }

        public new T MapThenValidateTo<T>(object? source)
            => base.MapThenValidateTo<T>(source);

        public new T? MapTo<T>(object? source)
            => base.MapTo<T>(source);

        public new T Validate<T>(T? model)
            => base.Validate(model);

        public new async Task<TResult> LogRequestResultAspect<TRequest, TResult>(string className, string methodName, TRequest request, Func<Task<TResult>> next)
            => await base.LogRequestResultAspect(className, methodName, request, next);

        public new async Task<TResult> LogRequestAspect<TRequest, TResult>(string className, string methodName, TRequest request, Func<Task<TResult>> next)
            => await base.LogRequestAspect(className, methodName, request, next);

        public new async Task LogRequestAspect<TRequest>(string className, string methodName, TRequest request, Func<Task> next)
            => await base.LogRequestAspect(className, methodName, request, next);

        public new async Task<TResult> LogAspect<TResult>(string className, string methodName, Func<Task<TResult>> next)
            => await base.LogAspect(className, methodName, next);

        public new async Task LogAspect(string className, string methodName, Func<Task> next)
            => await base.LogAspect(className, methodName, next);
    }

    private const string VALID_USERNAME = "username";
    private const string VALID_EMAIL = "email@x.y";
    private const string TEST_CLASS_NAME = nameof(TEST_CLASS_NAME);
    private const string TEST_METHOD_NAME = nameof(TEST_METHOD_NAME);

    protected readonly ServiceFactory _serviceFactory = new();

    private readonly Fixture _fixture = new();
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
        Assert.IsType<AutoMapperMappingException>(ex);
    }

    [Fact]
    public void MapThenValidateTo_Throws_IfDestinationIsInvalid()
    {
        var createUserRequest = _fixture.Create<CreateUserRequest>();

        var ex = Record.Exception(() => _mockProcessorBase.MapThenValidateTo<User>(createUserRequest));

        Assert.NotNull(ex);
        Assert.IsType<ValidationException>(ex);
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
        Assert.IsType<AutoMapperMappingException>(ex);
    }

    [Fact]
    public void Validate_DoesNotThrow_IfModelIsValid()
    {
        var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
        var user = _mockProcessorBase.MapTo<User>(createUserRequest);

        var ex = Record.Exception(() => _mockProcessorBase.Validate(user));

        Assert.Null(ex);
    }

    [Fact]
    public void Validate_Throws_WhenModelIsNull()
    {
        var ex = Record.Exception(() => _mockProcessorBase.Validate<object>(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task LogRequestResultAspect_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogRequestResultAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, null! as string,
            async () => await Task.FromResult(1)));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }


    [Fact]
    public async Task LogRequestResultAspect_ReturnsResult_WithRequestAndResult()
    {
        var result = await _mockProcessorBase.LogRequestResultAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, string.Empty,
            async () => await Task.FromResult(1));
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task LogRequestResultAspect_Throws_WhenNextThrows()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogRequestResultAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, string.Empty,
            async () => await Task.FromException<int>(new Exception())));
        Assert.NotNull(ex);
    }

    [Fact]
    public async Task LogRequestAspect_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogRequestAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, null! as string,
            () => Task.FromResult(1)));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task LogRequestAspect_ReturnsResult_WithRequestAndResult()
    {
        var result = await _mockProcessorBase.LogRequestAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, string.Empty,
            () => Task.FromResult(1));
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task LogRequestAspect_Throws_WhenNextThrows()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogRequestAspect<string, int>(
            TEST_CLASS_NAME, TEST_METHOD_NAME, string.Empty,
            () => throw new Exception()));
        Assert.NotNull(ex);
    }

    [Fact]
    public async Task LogRequestAspectWithNoResult_Throws_WithNullRequest()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogRequestAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, null! as string,
            () => Task.CompletedTask));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task LogRequestAspectWithNoResult_Returns_WithRequest()
    {
        await _mockProcessorBase.LogRequestAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, string.Empty,
            () => Task.CompletedTask);
    }

    [Fact]
    public async Task LogRequestAspectWithNoResult_Throws_WhenNextThrows()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogRequestAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME, string.Empty,
            () => throw new Exception()));
        Assert.NotNull(ex);
    }

    [Fact]
    public async Task LogAspect_ReturnsResult()
    {
        var result = await _mockProcessorBase.LogAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME,
            () => Task.FromResult(1));
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task LogAspect_Throws_WhenNextThrows()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogAspect<int>(
            TEST_CLASS_NAME, TEST_METHOD_NAME,
            () => throw new Exception()));
        Assert.NotNull(ex);
    }

    [Fact]
    public async Task LogAspectWithNoResult_Throws_WhenNextThrows()
    {
        var ex = await Record.ExceptionAsync(() => _mockProcessorBase.LogAspect(
            TEST_CLASS_NAME, TEST_METHOD_NAME,
            async () => await Task.FromException(new Exception())));
        Assert.NotNull(ex);
    }
}
