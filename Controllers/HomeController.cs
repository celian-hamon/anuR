using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using anuR.Models;
using anuR.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace anuR.Controllers;

public class HomeController : Controller
{
    //controller de la page de garde
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}