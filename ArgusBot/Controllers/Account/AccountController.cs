using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.Models;
using ArgusBot.Models.Account;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Login(string LoginInput, string passwordInput)
        {
            if (!_signInService.Authorize(LoginInput, passwordInput))
            {
                ViewBag.ErrorMessage = "Error! User not found. Please, try again.";

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
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.CreateNewUser(model.Login, model.Password);
                if (result)
                {
                    _signInService.Authorize(model.Login, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorMessage = "User not created.";
            }
            ViewBag.ErrorMessage = "Invalid input";

            return View();
        }
    }
}
