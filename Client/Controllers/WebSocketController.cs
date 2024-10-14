using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class WebSocketController(ILogger<WebSocketController> logger) : Controller
    {
        private readonly ILogger<WebSocketController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
    }
}
