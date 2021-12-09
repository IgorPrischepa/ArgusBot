﻿using ArgusBot.BL.Services.Interfaces;
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
        private readonly ITelegramBotClient botClient;
        private readonly ILogger<CheckListCleanerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer timer;


        public CheckListCleanerService(ITelegramBotClient client, ILogger<CheckListCleanerService> logger, IServiceProvider serviceProvider)
        {
            botClient = client;
            _logger = logger;
            _serviceProvider = serviceProvider;
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
            const byte inProgress = 0;

            _logger.LogInformation("Fetching list of captcha records");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ICheckListService checkList =
                  scope.ServiceProvider.GetRequiredService<ICheckListService>();

                IAsyncEnumerable<IEnumerable<Check>> currentList = checkList.GetCheckListWithStatus(inProgress, 500);

                _logger.LogInformation("Starting processing checklist.");

                await foreach (Check check in currentList)
                {
                    if ((DateTime.Now - check.SendingTime).TotalSeconds > TimeSpan.FromSeconds(30).TotalSeconds)
                    {
                        await botClient.DeleteMessageAsync(check.GroupId, check.QuestionMessageId);
                        await botClient.KickChatMemberAsync(check.GroupId, check.UserId);
                        await checkList.DeleteCheckForUser(check.UserId, check.GroupId);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cleaner was stopped");
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
            return Task.CompletedTask;
        }
    }
}
