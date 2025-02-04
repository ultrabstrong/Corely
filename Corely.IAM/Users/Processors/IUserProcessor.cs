﻿using Corely.IAM.Users.Models;

namespace Corely.IAM.Users.Processors;

internal interface IUserProcessor
{
    Task<CreateUserResult> CreateUserAsync(CreateUserRequest request);
    Task<User?> GetUserAsync(int userId);
    Task<User?> GetUserAsync(string userName);
    Task UpdateUserAsync(User user);
    Task<string?> GetUserAuthTokenAsync(int userId);
    Task<bool> IsUserAuthTokenValidAsync(int userId, string authToken);
    Task<string?> GetAsymmetricSignatureVerificationKeyAsync(int userId);
    Task<AssignRolesToUserResult> AssignRolesToUserAsync(AssignRolesToUserRequest request);
}
