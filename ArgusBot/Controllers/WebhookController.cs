using System.Threading.Tasks;

using ArgusBot.BL.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Telegram.Bot.Types;

namespace ArgusBot.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] IHandleUpdateService handleUpdateService,
                                              [FromBody] Update update)
        {
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
