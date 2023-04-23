using System.Security.Claims;
using anuR.Context;
using anuR.Models;
using anuR.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace anuR.Controllers;

public class SelfController : Controller
{
    private readonly MainContext _context;

    public SelfController(MainContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        Guid id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return RedirectToAction("User", "App", new { id });
    }
}