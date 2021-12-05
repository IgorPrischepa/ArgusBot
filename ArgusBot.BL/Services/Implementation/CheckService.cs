using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Models;
using ArgusBot.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task ChangeCheckStatusForUserAsync(int userId, int status)
        {
            var userToUpdate = await checkList.GetItemByUserIdAsync(userId);
            if (userToUpdate != null)
            {
                userToUpdate.Status = (StatusTypes)status;
                await checkList.UpdateAsync(userToUpdate);
            }
        }

        public async Task CreateCheckAsync(int telegramGroupId, int telegramUserId, string correctAnswer)
        {
            if (correctAnswer is null || correctAnswer == string.Empty) throw new ArgumentException("Answer can't be empty or null");
            if (telegramUserId == 0 || telegramUserId < 0) throw new ArgumentException("User id can't be zero or negative");
            if (telegramGroupId == 0 || telegramGroupId < 0) throw new ArgumentException("Group id can't be zero or negative");

            Check check = new Check
            {
                CorrectAnswer = correctAnswer,
                GroupId = telegramGroupId,
                UserId = telegramUserId
            };

            await checkList.CreateNewAsync(check);
        }

        public async Task DeleteCheckForUser(int userId)
        {
            if (userId == 0 || userId < 0) throw new ArgumentException("User id can't be zero or negative");
            await checkList.DeleteAsync(userId);
        }

        public async Task UpdateQuestionMsgId(int userId, int messageId)
        {
            if (userId == 0 || userId < 0) throw new ArgumentException("User id can't be zero or negative");
            if (messageId == 0 || messageId < 0) throw new ArgumentException("Message id can't be zero or negative");

            var itemToUpdate = await checkList.GetItemByUserIdAsync(userId);

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

        public async Task<List<Check>> GetCheckListWithStatus(int status)
        {
            return await checkList.GetCheckByStatusAsync((StatusTypes)status);
        }
    }
}
