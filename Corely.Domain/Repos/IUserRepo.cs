using Corely.Domain.Entities.Users;

namespace Corely.Domain.Repos
{
    public interface IUserRepo : IRepo<UserEntity>
    {
        UserEntity? GetByUserName(string userName);
        UserEntity? GetByEmail(string email);
        UserEntity? GetWithDetailsById(int userId);
        UserEntity? GetWithDetailsByUserName(string userName);
        UserEntity? GetWithDetailsByEmail(string email);
        bool DoesUserExist(string userName, string email);
    }
}
