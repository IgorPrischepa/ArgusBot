using ArgusBot.BL.Services;
using ArgusBot.BL.Services.Interfaces;
using ArgusBot.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ArgusBot.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;
        private readonly IQueryParser _queryParser;
        private readonly ICookieParser _cookieParser;

        public AccountController(IUserService userService, ISignInService signInService, IConfiguration configuration, IQueryParser queryParser, ICookieParser cookieParser)
        {
            _userService = userService;
            _signInService = signInService;
            Configuration = configuration;
            _queryParser = queryParser;
            _cookieParser = cookieParser;
        }
        private IConfiguration Configuration { get; }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVM() { RedirectUrl = $"{Configuration["data-auth"]}/Account/LoginByTelegram" });
        }
        [HttpGet]
        public IActionResult InitiateAttachingTelegramAccount()
        {
            return ViewComponent("AttachTelegram", new { canBeAttached = true });
        }
        [HttpGet]
        public async Task<IActionResult> AttachTelegramAccount()
        {
            var queryColect = _queryParser.ParseQueryString(HttpContext.Request.Query);
            if (queryColect.VerifyNotNull() && 
                queryColect.TryGetValue("id", out string telegramId) && 
                HttpContext.Request.Cookies.TryGetValue("identifier", out string userIdString))
            {
                var userId = (Guid)_cookieParser.ParseString<Guid>(userIdString);
                var isSuccesfull= await _userService.AddTelegramToAccountAsync(userId, telegramId);
                if (isSuccesfull)
                {
                    HttpContext.Response.Cookies.Append("attached_telegram", "true");
                    return RedirectToAction("Index","Home");
                }
            }
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
        public async Task<IActionResult> Login(string loginInput, string passwordInput)
        {
            bool isSuccesfull = await _signInService.AuthenticateAsync(loginInput, passwordInput);

            if (!isSuccesfull)
            {
                ViewBag.ErrorMessage = "Error! The user is not found or the password is incorrect. Please try again.";

                return View(new LoginVM() { RedirectUrl = $"{Configuration["data-auth"]}/Account/AttachTelegramAccount" });
            };

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View(new RegistrationViewModel() {RedirectUrl=$"{Configuration["data-auth"]}/Account/LoginByTelegram" });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await _userService.CreateNewUserAsync(model.Login, model.Password);

                if (result)
                {
                    await _signInService.AuthenticateAsync(model.Login, model.Password);
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessage = "This user already exists. Please use a different name.";
                return View();
            }

            ViewBag.ErrorMessage = "Invalid input";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
