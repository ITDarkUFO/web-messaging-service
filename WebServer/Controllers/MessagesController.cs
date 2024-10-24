using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using SharedLibrary.Resources;
using System.Text.Json;
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
            try
            {
                var messages = await _messagesRepository.GetMessages(startDate, endDate);

                if (messages.Count == 0)
                    return NoContent();

                return Ok(JsonSerializer.Serialize(messages));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, SharedResources.ExceptionError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm, Bind("MessageText, MessageIndex")] ChatMessage message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var sendingTime = await _messagesRepository.SendMessage(message);
                return Ok(sendingTime);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, SharedResources.ExceptionError);
            }
        }
    }
}
