using ArgusBot.BLL.DTO;
using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ArgusBot.BLL.Services.Implementation
{
    class UserService : IUserService
    {
        private readonly IUserRepository usersRepository;
        private readonly IHttpContextAccessor context;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            usersRepository = userRepository;
            context = httpContextAccessor;
        }

        public bool AddTelegramToAccount(Guid userGuid, string telegramId)
        {
            var user = usersRepository.GetUserById(userGuid);
            user.TelegramId = telegramId;
            usersRepository.Update(user);
            return true;
        }

        public bool Authorize(string login, string password)
        {
            var user = usersRepository.GetUserByLogin(login);
            var inputPassword = BCrypt.Net.BCrypt.HashPassword(password);

            if (user.Password != inputPassword)
            {
                return false;
            }

            return false;
        }

        public bool AuthorizeByTelegramAccount(string telegramId)
        {
            try
            {
                var user = usersRepository.GetUserByTelegramAccount(telegramId);

            }
            catch (ArgumentNullException ex)
            {
                if (CreateNewUserByTelegramAccount(telegramId))
                {
                    var user = usersRepository.GetUserByTelegramAccount(telegramId);

                    CreateAuthCoolkieAsync(new Profile
                    {
                        UserGuid = user.UserGuid,
                        Login = user.Login,
                        TelegramId = user.TelegramId
                    });

                    return true;
                }
            }
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

        public Profile GetUser(string login)
        {
            var user = usersRepository.GetUserByLogin(login);

            return new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            };
        }

        public Profile GetUser(Guid login)
        {
            var user = usersRepository.GetUserById(login);

            return new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            };
        }

        public Profile GetUserbyTelegramAccount(string telegramId)
        {
            var user = usersRepository.GetUserByTelegramAccount(telegramId);

            return new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            };
        }

        public async void Logout()
        {
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async void CreateAuthCoolkieAsync(Profile profile)
        {
            var claims = new List<Claim>
             {
                new Claim(ClaimTypes.Name, profile.Login)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                RedirectUri = "/Home"
            };

            await context.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
        }
    }
}
