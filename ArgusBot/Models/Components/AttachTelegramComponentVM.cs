using ArgusBot.BL.Services;

namespace ArgusBot.Models.Components
{
    public class AttachTelegramComponentVM
    {
        public IsAttachedTelegram IsAttachedTelegram { get; set; }
        public string RedirectUrl { get; set; }
    }
}
