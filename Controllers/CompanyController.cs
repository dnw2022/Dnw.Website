using Microsoft.AspNetCore.Mvc;

namespace Dnw.Website.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}