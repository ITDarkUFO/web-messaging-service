using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebServer.Models;
using WebServer.Repositories;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("messenger")]
    public class MessagesController(MessagesRepository messagesRepository) : ControllerBase
    {
        private readonly MessagesRepository _messagesRepository = messagesRepository;

        [HttpGet("get")]
        public async Task<IActionResult> GetMessages(DateTime startDate, DateTime endDate)
        {
            var messages = await _messagesRepository.GetMessages(startDate, endDate);
            return Ok(messages);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(ChatMessage message)
        {
            if (ModelState.IsValid)
            {
                var _message = await _messagesRepository.SendMessage(message);
                return Ok(_message);
            }
            else
                return BadRequest(ModelState.ValidationState);
        }
    }
}
