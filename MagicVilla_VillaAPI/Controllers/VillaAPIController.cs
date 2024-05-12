using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

public class VillaAPIController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}