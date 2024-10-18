using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
