using ArgusBot.BL.Services;
using ArgusBot.Models.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ArgusBot.ViewCompomemts
{
    public class AttachTelegramViewComponent : ViewComponent
    {
        public AttachTelegramViewComponent(IConfiguration config)
        {
            Configuration = config;
        }
        private IConfiguration Configuration { get; }
        public IViewComponentResult Invoke(bool canBeAttached)
        {
            if (HttpContext.Request.Cookies.ContainsKey("identifier"))
            {
                var redirectUrl = Configuration["data-auth"];
                if (canBeAttached) return View(new AttachTelegramComponentVM() { RedirectUrl = $"{ redirectUrl}/Account/AttachTelegramAccount", IsAttachedTelegram = IsAttachedTelegram.InProgress });
                if (HttpContext.Request.Cookies.TryGetValue("attached_telegram",out string value))
                {
                    if(value=="true") return View(new AttachTelegramComponentVM() { RedirectUrl = redirectUrl, IsAttachedTelegram = IsAttachedTelegram.Yes });
                    else return View(new AttachTelegramComponentVM() { RedirectUrl = redirectUrl, IsAttachedTelegram = IsAttachedTelegram.No });
                }
            }
            return View();
         }
    }
}

