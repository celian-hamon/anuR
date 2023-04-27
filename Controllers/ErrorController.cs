using Microsoft.AspNetCore.Mvc;

namespace anuR.Controllers;

public class ErrorController : Controller
{
    //controller de renvois d'erreur
    // GET
    public IActionResult NotFound()
    {
        return View();
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
}