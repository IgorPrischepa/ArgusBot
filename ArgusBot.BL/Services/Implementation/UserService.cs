using ArgusBot.BLL.DTO;
using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.DAL.Models;
using System;

using Microsoft.AspNetCore.Http;

namespace ArgusBot.BLL.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository usersRepository;

        public UserService(IUserRepository userRepository)
        {
            usersRepository = userRepository;
        }

        public bool AddTelegramToAccount(Guid userGuid, string telegramId)
        {
            var user = usersRepository.GetUserById(userGuid);
            user.TelegramId = telegramId;
            usersRepository.Update(user);
            return true;
        }

        public bool ChangePassword(Guid userGuid, string newPassword)
        {
            var user = usersRepository.GetUserById(userGuid);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            usersRepository.Update(user);
            return true;
        }

        public bool CreateNewUser(string login, string password)
        {
            var newUser = new User
            {
                Login = login,
                NormalizedLogin = login.ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            usersRepository.Create(newUser);
            return true;
        }

        public bool CreateNewUserByTelegramAccount(string telegramId)
        {

            var newUser = new User
            {
                Login = telegramId,
                NormalizedLogin = telegramId.ToLower(),
                Password = null,
                TelegramId = telegramId
            };
            usersRepository.Create(newUser);
            return true;
        }

        public Profile GetUserByLogin(string login)
        {
            var user = usersRepository.GetUserByLogin(login);

            return new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            };
        }

        public Profile GetUserByGuid(Guid login)
        {
            var user = usersRepository.GetUserById(login);

            return new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            };
        }

        public Profile GetUserByTelegramAccount(string telegramId)
        {
            var user = usersRepository.GetUserByTelegramAccount(telegramId);

            return new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            };
        }
    }
}
