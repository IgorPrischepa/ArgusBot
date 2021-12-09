using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Implementation
{
    public class CheckService : ICheckListService
    {
        private readonly ICheckListRepository checkList;

        public CheckService(ICheckListRepository checkListRepository)
        {
            checkList = checkListRepository;
        }

        public async Task ChangeCheckStatusForUserAsync(long userId, long groupId, StatusTypes status)
        {
            Check userToUpdate = await checkList.GetItemByUserAndGroupIdAsync(userId, groupId);
            if (userToUpdate != null)
            {
                userToUpdate.Status = status;
                await checkList.UpdateAsync(userToUpdate);
            }
        }

        public async Task CreateCheckAsync(long telegramGroupId, long telegramUserId, string correctAnswer)
        {
            if (string.IsNullOrEmpty(correctAnswer)) throw new ArgumentException("Answer can't be empty or null");
            if (telegramUserId <= 0) throw new ArgumentException("User id can't be zero or negative");
            var dateSentMessage = DateTime.Now;
            Check check = new Check
            {
                CorrectAnswer = correctAnswer,
                GroupId = telegramGroupId,
                UserId = telegramUserId,
                Status = StatusTypes.InProgress,
                SendingTime = dateSentMessage
            };

            await checkList.CreateNewAsync(check);
        }

        public async Task DeleteCheckForUser(long userId, long groupId)
        {
            if (userId <= 0) throw new ArgumentException("User id can't be zero or negative");
            if (groupId > 0) throw new ArgumentException("Group id can't positive or equal zero");
            await checkList.DeleteAsync(userId, groupId);
        }

        public async Task UpdateQuestionMsgId(long userId, long groupId, int messageId)
        {
            if (groupId > 0) throw new ArgumentException("Group id can't positive or equal zero");
            if (userId <= 0) throw new ArgumentException("User id can't be zero or negative");
            if (messageId <= 0) throw new ArgumentException("Message id can't be zero or negative");

            Check itemToUpdate = await checkList.GetItemByUserAndGroupIdAsync(userId, groupId);

            if (itemToUpdate is not null)
            {
                itemToUpdate.QuestionMessageId = messageId;
                await checkList.UpdateAsync(itemToUpdate);
            }
        }

        public async Task<ICollection<Check>> GetAllFromCheckListAsync()
        {
            return await checkList.GetAllCheckListsAsync();
        }

        public async IAsyncEnumerable<IEnumerable<Check>> GetCheckListWithStatus(StatusTypes status, int count)
        {
            if (count > 1000 && count <= 0) throw new ArgumentException("You cannot set a count value for chunks more than 1000 or less than 0");
            int readFiles = 0;
            while (true)
            {
                IEnumerable<Check> chunk = await Task.Run(() => checkList.GetCheckByStatusAsync(status, count, readFiles));
                var chunkCount = chunk.Count();
                if (chunkCount == 0)
                {
                    break;
                }
                readFiles += chunkCount;
                yield return chunk;
            }
        }

        public async Task<Check> GetCheckForUser(long telegramUserId, long groupId)
        {
            return await checkList.GetItemByUserAndGroupIdAsync(telegramUserId, groupId);
        }
    }
}
