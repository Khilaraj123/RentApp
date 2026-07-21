using Microsoft.AspNetCore.Mvc;

namespace RentApp.Presentation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}