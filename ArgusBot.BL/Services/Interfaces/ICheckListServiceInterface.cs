using ArgusBot.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface ICheckListServiceInterface
    {
        public Task CreateCheckAsync(Check checkItem);

        public Task ChangeCheckStatusForUserAsync(int userId, StatusTypes status);

        public Task DeleteCheckForUser(int userId);

        public Task UpdateQuestionMsgId(int userId, int messageId);

        public Task<ICollection<Check>> GetAllFromCheckListAsync();
    }
}
