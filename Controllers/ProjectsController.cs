using Microsoft.AspNetCore.Mvc;

namespace Dnw.Website.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
