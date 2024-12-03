using Corely.IAM.Models;

namespace Corely.IAM.Services;

public interface ISignInService
{
    Task<SignInResult> SignInAsync(SignInRequest request);
}
