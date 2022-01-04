using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace ArgusBot.BL.Services.Implementation
{
    public class CaptchaService : ICaptchaService
    {
        private readonly ITelegramBotClient _client;
        private readonly ICheckListService _checkListService;
        private readonly ILogger<ICaptchaService> _logger;
        public CaptchaService(ITelegramBotClient client, ICheckListService checkListService, ILogger<ICaptchaService> logger)
        {
            _client = client;
            _checkListService = checkListService;
            _logger = logger;
        }
        public async Task InitiateCaptchaProcess(IEnumerable<Telegram.Bot.Types.User> newUsers, long groupId)
        {
            newUsers.VerifyNotNull();
            if (newUsers.Any())
            {
                foreach (var user in newUsers)
                {
                    int firstTerm = GetRandomTerm();
                    int secondTerm = GetRandomTerm();
                    _logger.LogInformation($"Create a new captcha for user : {user.Id} in group {groupId}");
                    await _checkListService.CreateCheckAsync(groupId, user.Id, (firstTerm + secondTerm).ToString());
                    Message sentMessage = await _client.SendTextMessageAsync(groupId,
                                                    $"@{user.Username}, please write an answer for the following question: {firstTerm} + {secondTerm} = ?");
                    await _checkListService.UpdateQuestionMsgId(user.Id, groupId, sentMessage.MessageId);
                }
            }
        }
        public async Task ProcessMesagge(Message message)
        {
            Check currentCheck = await _checkListService.GetCheckForUser(message.From.Id, message.Chat.Id);
            if (currentCheck != null)
            {
                if (currentCheck.Status == StatusTypes.Successful)
                {
                    return;
                }
                string correctAnswer = currentCheck.CorrectAnswer;
                if (message.Text == correctAnswer)
                {
                    _logger.LogInformation($"User {message.From.Id} has succesfully passed captcha!");
                    await _checkListService.ChangeCheckStatusForUserAsync(message.From.Id, message.Chat.Id, StatusTypes.Successful);
                    await _client.DeleteMessageAsync(message.Chat.Id, currentCheck.QuestionMessageId);
                }
                await _client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                return;
            }
        }
        private int GetRandomTerm()
        {
            var rand = new Random();
            return rand.Next(0, 200);
        }
    }
}
