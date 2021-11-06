using ArgusBot.BLL.DTO;
using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.DAL.Models;
using System;
using System.Threading.Tasks;

namespace ArgusBot.BLL.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository usersRepository;

        public UserService(IUserRepository userRepository)
        {
            usersRepository = userRepository;
        }

        public async Task<bool> AddTelegramToAccountAsync(Guid userGuid, string telegramId)
        {
            User user = await usersRepository.GetUserByIdAsync(userGuid);

            user.TelegramId = telegramId;

            await usersRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userGuid, string newPassword)
        {
            User user = await usersRepository.GetUserByIdAsync(userGuid);

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await usersRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> CreateNewUserAsync(string login, string password)
        {
            User user = await usersRepository.GetUserByLoginAsync(login);

            if (user != null)
            {
                return false;
            }

            var newUser = new User
            {
                Login = login,
                NormalizedLogin = login.ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await usersRepository.CreateAsync(newUser);

            return true;
        }

        public async Task<bool> CreateNewUserByTelegramAccountAsync(string telegramId)
        {
            User user = await usersRepository.GetUserByTelegramAccountAsync(telegramId);

            if (user == null)
            {
                var newUser = new User
                {
                    Login = telegramId,
                    NormalizedLogin = null,
                    Password = null,
                    TelegramId = telegramId
                };
                await usersRepository.CreateAsync(newUser);

                return true;
            }

            return false;
        }

        public async Task<Profile> GetUserByLoginAsync(string login)
        {
            User user = await usersRepository.GetUserByLoginAsync(login);

            if (user != null)
            {
                return new Profile
                {
                    UserGuid = user.UserGuid,
                    Login = user.Login,
                    TelegramId = user.TelegramId
                };
            }
            return null;
        }

        public async Task<Profile> GetUserByGuidAsync(Guid login)
        {
            User user = await usersRepository.GetUserByIdAsync(login);

            if (user != null)
            {
                return new Profile
                {
                    UserGuid = user.UserGuid,
                    Login = user.Login,
                    TelegramId = user.TelegramId
                };
            }
            return null;
        }

        public async Task<Profile> GetUserByTelegramAccountAsync(string telegramId)
        {
            User user = await usersRepository.GetUserByTelegramAccountAsync(telegramId);

            if (user != null)
            {
                return new Profile
                {
                    UserGuid = user.UserGuid,
                    Login = user.Login,
                    TelegramId = user.TelegramId
                };
            }
            return null;
        }
    }
}
