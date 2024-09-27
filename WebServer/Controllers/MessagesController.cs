using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebServer.Models;
using WebServer.Repositories;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController(MessagesRepository messagesRepository) : ControllerBase
    {
        private readonly MessagesRepository _messagesRepository = messagesRepository;

        [HttpGet]
        public async Task<IActionResult> GetMessages(DateTime startDate, DateTime endDate)
        {
            var messages = await _messagesRepository.GetMessages(startDate, endDate);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([Bind("MessageText, MessageTimestamp, MessageIndex")] ChatMessage message)
        {
            if (ModelState.IsValid)
            {
                await _messagesRepository.SendMessage(message);
                return Ok();
            }
            else
                return BadRequest(ModelState.ValidationState);
        }
    }
}
