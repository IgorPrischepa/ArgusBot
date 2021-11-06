using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.Models.Account;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArgusBot.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;

        public AccountController(IUserService userService, ISignInService signInService)
        {
            _userService = userService;
            _signInService = signInService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LoginByTelegram(string telegramId)
        {
            bool isSuccesfull = await _signInService.AuthorizeByTelegramAccountAsync(telegramId);

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

                return View();
            };

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
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
