using ArgusBot.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface ICheckListService
    {
        public Task CreateCheckAsync(long telegramGroupId, long telegramUserId, string correctAnswer);

        public Task ChangeCheckStatusForUserAsync(long userId, byte status);

        public Task DeleteCheckForUser(long userId);

        public Task UpdateQuestionMsgId(long userId, int messageId);

        public Task<List<Check>> GetCheckListWithStatus(byte status);

        public Task<ICollection<Check>> GetAllFromCheckListAsync();
    }
}
