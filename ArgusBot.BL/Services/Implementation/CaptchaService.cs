using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
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

        public CaptchaService(ITelegramBotClient client, ICheckListService checkListService)
        {
            _client = client;
            _checkListService = checkListService;
        }
        public async Task InitiateCaptchaProcess(IEnumerable<Telegram.Bot.Types.User> newUsers, long groupId)
        {
            newUsers.VerifyNotNull();
            if (newUsers.Count() != 0)
            {
                foreach (var user in newUsers)
                {
                    int firstTerm = GetRandomTerm();
                    int secondTerm = GetRandomTerm();
                    await _checkListService.CreateCheckAsync(groupId, user.Id, (firstTerm + secondTerm).ToString());
                    var sentMessage = await _client.SendTextMessageAsync(groupId, 
                                                    $"{user.Username}, please write an answer for the following question: {firstTerm} + {secondTerm} = ?");
                    await _checkListService.UpdateQuestionMsgId(user.Id, sentMessage.MessageId);
                }
            }
        }

        public async Task ProcessMesagge(Message message)
        {
            IEnumerable<Check> checkNotes =  await _checkListService.GetAllFromCheckListAsync();
            Check currentCheck = checkNotes.SingleOrDefault(u => u.UserId == message.From.Id);
            if (currentCheck != null)
            {
                var correctAnswer = currentCheck.CorrectAnswer;
                if (message.Text == correctAnswer)
                {
                    await _checkListService.ChangeCheckStatusForUserAsync(message.From.Id, (byte)StatusTypes.Successful);
                    await _client.DeleteMessageAsync(message.Chat.Id, currentCheck.QuestionMessageId);
                }
                await _client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            }
            return;
        }

        private int GetRandomTerm()
        {
            var rand = new Random();
            return rand.Next(0, 200);
        }
    }
}
