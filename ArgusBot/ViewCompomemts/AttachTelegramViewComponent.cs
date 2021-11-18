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
        public IViewComponentResult InvokeAsync()
        {
            if (HttpContext.Request.Cookies.ContainsKey("identifier"))
            {
                var redirectUrl = Configuration["data-auth"];
                if (HttpContext.Request.Cookies.TryGetValue("isattachedtelegram", out string isAttachedString))
                {
                    var isAttached = isAttachedString == "1" ? IsAttachedTelegram.Yes : IsAttachedTelegram.InProgress;
                    return View(new AttachTelegramComponentVM() { IsAttachedTelegram = isAttached,RedirectUrl=redirectUrl });
                }
                return View(new AttachTelegramComponentVM() { RedirectUrl = redirectUrl, IsAttachedTelegram = IsAttachedTelegram.No });
            }
            return View();
         }
    }
}

