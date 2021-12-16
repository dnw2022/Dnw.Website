using Microsoft.AspNetCore.Mvc;

namespace Dnw.Website.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
