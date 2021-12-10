using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<CheckListCleanerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;


        public CheckListCleanerService(ITelegramBotClient client, ILogger<CheckListCleanerService> logger, IServiceProvider serviceProvider)
        {
            _botClient = client;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting processing checklist.");

            _timer = new Timer(DeletingFailedCpathcaChecks, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private async void DeletingFailedCpathcaChecks(object state)
        {
            _logger.LogInformation("Fetching list of captcha records");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ICheckListService checkList =
                  scope.ServiceProvider.GetRequiredService<ICheckListService>();

                IAsyncEnumerable<IEnumerable<Check>> currentList = checkList.GetCheckListWithStatus(StatusTypes.InProgress, 500);

                _logger.LogInformation("Starting processing checklist.");

                await foreach (Check check in currentList)
                {
                    if ((DateTime.Now - check.SendingTime).TotalSeconds > 30)
                    {
                        await _botClient.DeleteMessageAsync(check.GroupId, check.QuestionMessageId);
                        await _botClient.KickChatMemberAsync(check.GroupId, check.UserId);
                        await checkList.DeleteCheckForUser(check.UserId, check.GroupId);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cleaner was stopped");
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer.Dispose();
            return Task.CompletedTask;
        }
    }
}
