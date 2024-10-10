using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
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
        public async Task<IActionResult> SendMessage([FromForm, Bind("MessageText, MessageIndex")] ChatMessage message)
        {
            if (ModelState.IsValid)
            {
                var sendingTime = await _messagesRepository.SendMessage(message);
                return Ok(sendingTime);
            }
            else
                return BadRequest(ModelState.ValidationState);
        }
    }
}
