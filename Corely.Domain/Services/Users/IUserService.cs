﻿using Corely.Domain.Models.Users;

namespace Corely.Domain.Services.Users
{
    public interface IUserService
    {
        Task<CreateUserResult> CreateUser(CreateUserRequest createUserRequest);

    }
}
