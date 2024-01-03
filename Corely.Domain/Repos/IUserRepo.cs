using Corely.Domain.Entities.Users;

namespace Corely.Domain.Repos
{
    public interface IUserRepo : IRepo<UserEntity>
    {
        Task<UserEntity?> GetByUserName(string userName);
        Task<UserEntity?> GetByEmail(string email);
        Task<UserEntity?> GetWithDetailsById(int userId);
        Task<UserEntity?> GetWithDetailsByUserName(string userName);
        Task<UserEntity?> GetWithDetailsByEmail(string email);
        Task<bool> DoesUserExist(string userName, string email);
    }
}
