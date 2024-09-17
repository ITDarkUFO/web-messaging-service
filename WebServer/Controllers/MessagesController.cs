using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebServer.Models;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("messenger")]
    public class MessagesController : ControllerBase
    {
        [HttpGet("get")]
        public async Task<IActionResult> GetMessages(DateTime startDate, DateTime endDate)
        {
            return Ok("Заглушка");
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(ChatMessage message)
        {
            if (ModelState.IsValid)
                return Ok();
            else
                return BadRequest(ModelState.ValidationState);
        }
    }
}
