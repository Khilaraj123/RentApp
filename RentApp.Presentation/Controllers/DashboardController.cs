using Microsoft.AspNetCore.Mvc;

namespace RentApp.Presentation.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index(string? role = null)
    {
        ViewData["Title"] = "Host Dashboard";
        ViewData["PreviewRole"] = role;
        return View();
    }
}
