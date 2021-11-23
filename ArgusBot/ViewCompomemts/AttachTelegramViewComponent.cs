using ArgusBot.BL.Services;
using ArgusBot.Models.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ArgusBot.ViewCompomemts
{
    public class AttachTelegramViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        public AttachTelegramViewComponent(IConfiguration config)
        {
            _configuration = config;
        }
        public IViewComponentResult Invoke()
        {
            var redirectUrl = _configuration["data-auth"];
            if (HttpContext.Request.Cookies.ContainsKey("identifier"))
            {
                if (HttpContext.Request.Cookies.TryGetValue("attached_telegram", out string value))
                {
                    if (value == "true") 
                    { 
                        return View(new AttachTelegramComponentVM() { RedirectUrl = redirectUrl, 
                                                                      IsAttachedTelegram = IsAttachedTelegram.Yes }); 
                    }
                }
            }
            return View(new AttachTelegramComponentVM() { RedirectUrl = $"{ redirectUrl}/Account/AttachTelegramAccount", 
                                                         IsAttachedTelegram = IsAttachedTelegram.No });
        }
    }
}

