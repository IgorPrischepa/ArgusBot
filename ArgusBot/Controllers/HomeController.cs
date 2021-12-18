using ArgusBot.BL.DTO;
using ArgusBot.BL.Services.Interfaces;
using ArgusBot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;


namespace ArgusBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGroupSettingsService _groupSettings;

        public HomeController(IGroupSettingsService groupSettings)
        {
            _groupSettings = groupSettings;

        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int GroupId)
        {
            var groupSettings = await _groupSettings.GetGroupSettingsAsync(GroupId);

            if (groupSettings == default)
            {
                return BadRequest();
            }

            return View(groupSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(GroupSettingsDTO settings)
        {
            if (await _groupSettings.UpdateSettingsAsync(settings))
            {
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            };

            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
