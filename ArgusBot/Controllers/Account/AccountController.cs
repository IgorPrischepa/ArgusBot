﻿using ArgusBot.BL.Services.Interfaces;
using ArgusBot.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;
        private readonly IQueryParser _queryParser;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IUserService userService,
                                 ISignInService signInService,
                                 IConfiguration configuration,
                                 IQueryParser queryParser,
                                 ILogger<AccountController> logger)
        {
            _userService = userService;
            _signInService = signInService;
            _configuration = configuration;
            _queryParser = queryParser;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.RedirectUrl = $"{_configuration["redirect-url"]}/Account/LoginByTelegram";
            return View(new LoginVM());
        }
        [HttpGet]
        public async Task<IActionResult> AttachTelegramAccount()
        {
            Dictionary<string, string> queryColect = _queryParser.ParseQueryString(HttpContext.Request.Query);
            if (queryColect != null &&
                queryColect.TryGetValue("id", out string telegramId) &&
                HttpContext.Request.Cookies.TryGetValue("identifier", out string userIdString))
            {
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    var isSuccesfull = await _userService.AddTelegramToAccountAsync(userId, telegramId);
                    if (isSuccesfull)
                    {
                        HttpContext.Response.Cookies.Append("attached_telegram", "true");
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    _logger.LogError("Invalid format of Guid string");
                }
            }
            _logger.LogError("Cannot attach telegram account to this user!");
            ViewBag.ErrorMessage = "Cannot attach a telegram profile to the current web-account";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LoginByTelegram()
        {
            bool isSuccesfull = await _signInService.AuthenticateByTelegramAccountAsync(_queryParser.ParseQueryString(HttpContext.Request.Query));
            if (!isSuccesfull)
            {
                ViewBag.ErrorMessage = "Error! Something wrong, please try again.";

                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginVM loginVM)
        {
            ViewBag.RedirectUrl = $"{_configuration["redirect-url"]}/Account/LoginByTelegram";
            if (ModelState.IsValid)
            {
                bool isSuccesfull = await _signInService.AuthenticateAsync(loginVM.Login, loginVM.Password);

                if (!isSuccesfull)
                {
                    ViewBag.ErrorMessage = "Error! The user is not found or the password is incorrect. Please try again.";

                    return View(loginVM);
                };

                return RedirectToAction("Index", "Home");
            }
            return View(loginVM);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            ViewBag.RedirectUrl = $"{_configuration["redirect-url"]}/Account/LoginByTelegram";
            return View(new RegistrationViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            ViewBag.RedirectUrl = $"{_configuration["redirect-url"]}/Account/LoginByTelegram";
            if (ModelState.IsValid)
            {
                bool result = await _userService.CreateNewUserAsync(model.Login, model.Password);

                if (result)
                {
                    await _signInService.AuthenticateAsync(model.Login, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorMessage = "This user already exists. Please use a different name.";
                return View(model);
            }

            ViewBag.ErrorMessage = "Invalid input";
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
