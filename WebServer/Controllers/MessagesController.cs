using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetMessages(DateTime startDate, DateTime endDate)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatMessage message)
        {
            return Ok();
        }
    }
}
