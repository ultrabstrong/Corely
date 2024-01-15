﻿using AutoFixture;
using Corely.Domain.Entities.Auth;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    public abstract class BasicAuthRepoTestsBase : RepoTestsBase<BasicAuthEntity>
    {
        public BasicAuthRepoTestsBase()
        {
            fixture.Customize<BasicAuthEntity>(c => c
                .Without(x => x.User));
        }

        [Fact]
        public async Task Create_ThenGetByUserId_ShouldReturnAddedBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await Repo.CreateAsync(basicAuth);
            var result = await Repo.GetAsync(a => a.UserId == basicAuth.UserId);

            Assert.Equal(basicAuth, result);
        }

        [Fact]
        public async Task Create_ThenGetByUserName_ShouldReturnAddedBasicAuth()
        {
            var basicAuth = fixture.Create<BasicAuthEntity>();

            await Repo.CreateAsync(basicAuth);
            var result = await Repo.GetAsync(a => a.Username == basicAuth.Username);

            Assert.Equal(basicAuth, result);
        }
    }
}
