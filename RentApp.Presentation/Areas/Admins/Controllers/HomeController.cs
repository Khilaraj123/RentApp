using Microsoft.AspNetCore.Mvc;

namespace RentApp.Presentation.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
