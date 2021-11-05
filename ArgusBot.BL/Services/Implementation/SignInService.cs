using ArgusBot.BLL.DTO;
using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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

        public bool Authorize(string login, string password)
        {
            var user = _userRepository.GetUserByLogin(login);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    CreateAuthCoolkieAsync(new Profile()
                    {
                        Login = user.Login,
                        UserGuid = user.UserGuid,
                        TelegramId = user.TelegramId
                    });

                    return true;
                }
            }

            return false;
        }

        public bool AuthorizeByTelegramAccount(string telegramId)
        {
            var user = _userRepository.GetUserByTelegramAccount(telegramId);

            if (user == null)
            {
                if (_userService.CreateNewUserByTelegramAccount(telegramId))
                {
                    AuthorizeByTelegramAccount(telegramId);
                }
                else
                {
                    return false;
                }
            }

            CreateAuthCoolkieAsync(new Profile
            {
                UserGuid = user.UserGuid,
                Login = user.Login,
                TelegramId = user.TelegramId
            });

            return true;
        }

        public async void Logout()
        {
            await _context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async void CreateAuthCoolkieAsync(Profile profile)
        {
            bool HasTelegramAccount = profile.TelegramId != null;

            var claims = new List<Claim>
             {
                new Claim(ClaimTypes.Name, profile.Login),
                new Claim(ClaimValueTypes.Boolean, HasTelegramAccount.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await _context.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
        }
    }
}
