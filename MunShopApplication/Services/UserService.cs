﻿using MunShopApplication.Helpers;
using MunShopApplication.Commons;
using MunShopApplication.DTOs;
using MunShopApplication.Entities;
using MunShopApplication.Repository.SQLServer;
using DirectoryPermissionManagement.Helpers;
using MunShopApplication.Configs;

namespace MunShopApplication.Services
{
    public class UserService
    {
        private readonly SQLServerUserRepository _userRepository;
        private readonly JWTConfig _jwtConfig;


        public UserService(SQLServerUserRepository userRepository, JWTConfig jwtConfig)
        {
            _userRepository = userRepository;
            _jwtConfig = jwtConfig;
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
        public async Task<UserResponse?> Login(User user)
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                return null;
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            var userInDb = await _userRepository.GetByUsername(user.Username);

            if (userInDb == null)
            {
                return null;
            }

            var passwordHashed = StringHelper.HashPassword(user.Password, Convert.FromBase64String(userInDb.Salt));

            if (passwordHashed != userInDb.Password)
            {
                return null;
            }

            var token = TokenHelper.GenerateToken(userInDb, _jwtConfig);

            return new UserResponse()
            {
                Id = userInDb.Id,
                Name = userInDb.Name,
                Email = userInDb.Email,
                RoleId = userInDb.RoleId,
                Token = token
            };

        }
    }
}
