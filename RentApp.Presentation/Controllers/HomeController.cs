using Microsoft.AspNetCore.Mvc;

namespace RentApp.Presentation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult NearYou()
    {
        ViewData["Title"] = "Near You";
        return View("Index");
    }

    public IActionResult Rooms()
    {
        ViewData["Title"] = "Rooms";
        return View("Index");
    }

    public IActionResult Shops()
    {
        ViewData["Title"] = "Shops";
        return View("Index");
    }
}