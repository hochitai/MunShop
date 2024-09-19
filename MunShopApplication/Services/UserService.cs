using DirectoryPermissionManagement.Helpers;
using MunShopApplication.Commons;
using MunShopApplication.DTOs;
using MunShopApplication.Entities;
using MunShopApplication.Repository.SQLServer;

namespace MunShopApplication.Services
{
    public class UserService
    {
        private readonly SQLServerUserRepository _userRepository;

        public UserService(SQLServerUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse?> Add(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return null;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                return null;
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                return null;
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            if (await _userRepository.GetByUsername(user.Username) != null)
            {
                return null;
            }

            user.Id = Guid.NewGuid();
            user.RoleId = (int) RoleEnum.User;

            var salt = StringHelper.CreateSalt();

            user.Salt = Convert.ToBase64String(salt);

            var passwordHashed = StringHelper.HashPassword(user.Password, salt);
            user.Password = passwordHashed;

            var result = await _userRepository.Add(user);

            return new UserResponse() 
            { 
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId,
            };

        }

    }
}
