using Microsoft.AspNetCore.Mvc;

namespace Dnw.Website.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}