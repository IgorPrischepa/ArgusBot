using ArgusBot.BLL.DTO;
using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArgusBot.BLL.Services.Implementation
{
    public class SignInService : ISignInService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public SignInService(IHttpContextAccessor httpContextAccessor, IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _context = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<bool> AuthenticateAsync(string login, string password)
        {
            User user = await _userRepository.GetUserByLoginAsync(login);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    await CreateAuthCookieAsync(new Profile() { Login = user.Login, UserGuid = user.UserGuid, TelegramId = user.TelegramId });

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> AuthenticateByTelegramAccountAsync(string telegramId)
        {
            User user = await _userRepository.GetUserByTelegramAccountAsync(telegramId);

            if (user == null)
            {
                bool isSuccsesfull = await _userService.CreateNewUserByTelegramAccountAsync(telegramId);
                if (isSuccsesfull)
                {
                    await AuthenticateByTelegramAccountAsync(telegramId);
                }
                else
                {
                    return false;
                }
            }

            await CreateAuthCookieAsync(new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            });

            return true;
        }

        public async Task LogoutAsync()
        {
            await _context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task CreateAuthCookieAsync(Profile profile)
        {
            bool hasTelegramAccount = profile.TelegramId != null;

            var claims = new List<Claim>
             {
                new Claim(ClaimTypes.Name, profile.Login),
                new Claim(ClaimValueTypes.Boolean, hasTelegramAccount.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(180),

            };

            await _context.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
        }
    }
}
