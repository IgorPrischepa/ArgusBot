using ArgusBot.BL.DTO;
using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Telegram.Bot.Extensions.LoginWidget;

namespace ArgusBot.BL.Services.Implementation
{
    public class SignInService : ISignInService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ISignInService> _logger;
        private readonly IConfiguration _configuration;

        public SignInService(IHttpContextAccessor httpContextAccessor,
                             IUserService userService,
                             IUserRepository userRepository,
                             IConfiguration configuration,
                             ILogger<ISignInService> logger)
        {
            _userService = userService;
            _context = httpContextAccessor;
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> AuthenticateAsync(string login, string password)
        {
            User user = await _userRepository.GetUserByLoginAsync(login);
            AddAttachTelegramCookies(user.TelegramId);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    var newUser = new ProfileDTO() { Login = user.Login, UserGuid = user.UserGuid, TelegramId = user.TelegramId };
                    await CreateAuthCookieAsync(newUser);
                    await AddIdCookies(newUser.Login);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AuthenticateByTelegramAccountAsync(Dictionary<string, string> queryString)
        {
            queryString.VerifyNotNull("Collection of query items cannot be null!");
            if (queryString.TryGetValue("id", out string id))
            {
                _logger?.LogInformation($"Authentication process for user:{queryString["id"]} has started");
                ProfileDTO authUser = await _userService.GetUserByTelegramAccountAsync(id);
                if (authUser == null)
                {
                    await AuthorizeViaTelegram(queryString, authUser);
                    _logger?.LogInformation($"User:{authUser.TelegramId} has succesfully authorized!");
                    await AddIdCookies(authUser.Login);
                    return true;
                }
                else
                {
                    if (queryString.TryGetValue("username", out string username))
                    {
                        ProfileDTO newUser = await _userService.CreateNewUserByTelegramAccountAsync(id, username);
                        if (newUser == null)
                        {
                            _logger?.LogInformation($"It`s created a new user by data from telegram account");
                            _logger?.LogInformation($"Authentication process for user:{queryString["id"]} has started");
                            await AuthorizeViaTelegram(queryString, newUser);
                            _logger?.LogInformation($"User:{newUser.TelegramId} has succesfully authorized!");
                            await AddIdCookies(newUser.Login);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public async Task LogoutAsync()
        {
            await _context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task CreateAuthCookieAsync(ProfileDTO profile)
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
        private async Task AuthorizeViaTelegram(Dictionary<string, string> data, ProfileDTO authorizedUser)
        {
            var widget = new LoginWidget(_configuration["bot-token"]);
            widget.AllowedTimeOffset = 900;
            Authorization result = widget.CheckAuthorization(data);
            switch (result)
            {
                case Authorization.Valid:
                    {
                        await CreateAuthCookieAsync(authorizedUser);
                        break;
                    }
                case Authorization.InvalidHash:
                    {
                        _logger?.LogError("Computed hash in service was invalid!");
                        throw new InvalidOperationException("Computed hash is invalid!");
                    }
                case Authorization.MissingFields:
                    {
                        _logger?.LogError("Authorization data from telegram was invalid!");
                        throw new InvalidOperationException("Authorization data in invalid!");
                    }
                case Authorization.TooOld:
                    {
                        _logger.LogWarning("Authorization process is not avalaible now");
                        throw new TimeoutException("Authorization process has stopped becaue of time for authorization is over!");
                    }
                case Authorization.InvalidAuthDateFormat:
                    {
                        _logger.LogError("Authorization date has not correct data!");
                        throw new InvalidOperationException("Authorization date for this process is not correct!");
                    }
            }
        }
        private async Task AddIdCookies(string login)
        {
            ProfileDTO checkedUser = await _userService.GetUserByLoginAsync(login);
            if (checkedUser == null)
            {
                if (checkedUser.UserGuid != Guid.Empty)
                    _context.HttpContext.Response.Cookies.Append("identifier", checkedUser.UserGuid.ToString());
                AddAttachTelegramCookies(checkedUser.TelegramId);
                return;
            }
        }
        private void AddAttachTelegramCookies(string telegramId)
        {
            if (string.IsNullOrEmpty(telegramId))
                _context.HttpContext.Response.Cookies.Append("attached_telegram", "false");
            else _context.HttpContext.Response.Cookies.Append("attached_telegram", "true");
        }
    }
}
