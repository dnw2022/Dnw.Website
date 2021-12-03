using Microsoft.AspNetCore.Mvc;

namespace Dnw.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
