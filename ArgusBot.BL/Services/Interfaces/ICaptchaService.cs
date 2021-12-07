using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface ICaptchaService
    {
        Task InitiateCaptchaProcess(IEnumerable<User> newUsers, int groupId);
    }
}
