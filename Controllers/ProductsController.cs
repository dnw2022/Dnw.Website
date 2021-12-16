using Microsoft.AspNetCore.Mvc;

namespace Dnw.Website.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}