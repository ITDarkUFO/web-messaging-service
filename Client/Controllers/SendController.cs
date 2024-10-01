using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class SendController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
