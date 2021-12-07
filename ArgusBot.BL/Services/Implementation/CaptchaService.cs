using ArgusBot.BL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ArgusBot.BL.Services.Implementation
{
    public class CaptchaService : ICaptchaService
    {
        private readonly ITelegramBotClient _client;
        private readonly ICheckListService _checkListService;

        public CaptchaService(ITelegramBotClient client, ICheckListService checkListService)
        {
            _client = client;
            _checkListService = checkListService;
        }
        public async Task InitiateCaptchaProcess(IEnumerable<User> newUsers, int groupId)
        {
            newUsers.VerifyNotNull();
            if (newUsers.Count() != 0)
            {
                foreach (var user in newUsers)
                {
                    int firstTerm = GetRandomTerm();
                    int secondTerm = GetRandomTerm();
                    await _checkListService.CreateCheckAsync(groupId, user.Id, (firstTerm + secondTerm).ToString());
                }
            }
        }
        private int GetRandomTerm()
        {
            var rand = new Random();
            return rand.Next(0, 200);
        }
    }
}
