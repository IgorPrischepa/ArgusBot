using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using ArgusBot.Services;

namespace ArgusBot.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
                                              [FromBody] Update update)
        {
            var body = HttpContext.Request.Body;
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
