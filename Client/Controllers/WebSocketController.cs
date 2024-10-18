using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class WebSocketController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        public IActionResult Index()
        {
            ViewBag.URL = _configuration.GetValue<string>("SERVER_URL");
            return View();
        }
    }
}
