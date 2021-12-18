using System.Threading.Tasks;

using ArgusBot.BL.DTO;

namespace ArgusBot.BL.Services.Interfaces
{
    public interface IGroupSettingsService
    {
        Task<bool> isCapthcaEnabledAsync(long groupId);

        Task<GroupSettingsDTO> GetGroupSettingsAsync(long groupId);

        Task<bool> UpdateSettingsAsync(GroupSettingsDTO settings);
    }
}
