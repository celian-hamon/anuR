using Microsoft.AspNetCore.Mvc;

namespace anuR.Controllers;

public class ErrorController : Controller
{
    // GET
    public IActionResult NotFound()
    {
        return View();
    }
}