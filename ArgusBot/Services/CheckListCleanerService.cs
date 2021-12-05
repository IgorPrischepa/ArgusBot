using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ArgusBot.Services.Implementations
{
    public class CheckListCleanerService : IHostedService
    {
        private readonly ITelegramBotClient botClient;
        private readonly ILogger<CheckListCleanerService> _logger;
        private readonly ICheckListService checkListService;
        private Timer timer;


        public CheckListCleanerService(ITelegramBotClient client, ILogger<CheckListCleanerService> logger)
        {
            botClient = client;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting processing checklist.");

            timer = new Timer(DeletingFailedCpathcaChecks, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private async void DeletingFailedCpathcaChecks(object state)
        {
            const byte inProgress = 1;

            _logger.LogInformation("Fetching list of  capthca records");

            List<Check> currentList = await checkListService.GetCheckListWithStatus(inProgress);

            _logger.LogInformation("Starting processing checklist.");

            foreach (Check check in currentList)
            {
                if ((check.SendingTime - DateTime.Now) > TimeSpan.FromSeconds(30))
                {
                    await botClient.DeleteMessageAsync(check.GroupId, check.QuestionMessageId);
                    await botClient.KickChatMemberAsync(check.GroupId, check.UserId);
                    await checkListService.DeleteCheckForUser(check.UserId);
                }
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
