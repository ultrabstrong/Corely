using AutoMapper;
using Corely.Domain.Entities.Users;
using Corely.Domain.Exceptions;
using Corely.Domain.Models.Users;
using Corely.Domain.Repos;

namespace Corely.Domain.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public void Create(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            if (_userRepo.DoesUserExist(userEntity.Username, userEntity.Email))
            {
                throw new UserServiceException() { Reason = UserServiceException.Reason.UserAlreadyExists };
            }
            _userRepo.Create(userEntity);
        }
    }
}
