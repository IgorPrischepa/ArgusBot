using ArgusBot.BL.DTO;
using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository usersRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IUserService> _logger;
        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<IUserService> logger)
        {
            usersRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
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
                NormalizedLogin=login.ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await usersRepository.CreateAsync(newUser);

            return true;
        }

        public async Task<ProfileDTO> CreateNewUserByTelegramAccountAsync(string telegramId, string userName)
        {
            _logger.LogInformation("It`s initialized a process to create a new user by data from telegram");
            User userDb = await usersRepository.GetUserByTelegramAccountAsync(telegramId);
            var userDTO = _mapper.Map<ProfileDTO>(userDb);
            if (!userDTO.VerifyNotNull(throwException:false))
            {
                userDTO = new ProfileDTO();
                userDTO.TelegramId = telegramId;
                userDTO.Login = userName;
                var newUser = _mapper.Map<User>(userDTO);
                if (newUser.VerifyNotNull("It`s happened a error during mapping process!"))
                {
                    newUser.Password = GenerateRandomPassword();
                    await usersRepository.CreateAsync(newUser);
                    _logger.LogInformation($"It`s created a new user : {newUser.Login}");
                    return userDTO;
                }
            }
            return userDTO;
        }
        public async Task<ProfileDTO> GetUserByLoginAsync(string login)
        {
            User user = await usersRepository.GetUserByLoginAsync(login);

            if (user != null)
            {
                return new ProfileDTO
                {
                    UserGuid = user.UserGuid,
                    Login = user.Login,
                    TelegramId = user.TelegramId
                };
            }
            return null;
        }

        public async Task<DTO.ProfileDTO> GetUserByGuidAsync(Guid login)
        {
            User user = await usersRepository.GetUserByIdAsync(login);

            if (user != null)
            {
                return new ProfileDTO
                {
                    UserGuid = user.UserGuid,
                    Login = user.Login,
                    TelegramId = user.TelegramId
                };
            }
            return null;
        }

        public async Task<ProfileDTO> GetUserByTelegramAccountAsync(string telegramId)
        {
            User user = await usersRepository.GetUserByTelegramAccountAsync(telegramId);

            if (user != null)
            {
                return new ProfileDTO
                {
                    UserGuid = user.UserGuid,
                    Login = user.Login,
                    TelegramId = user.TelegramId
                };
            }
            return null;
        }
        private string GenerateRandomPassword(int length = 32)
        {
            using(var rng=new RNGCryptoServiceProvider())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
