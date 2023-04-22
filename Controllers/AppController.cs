using anuR.Context;
using anuR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace anuR.Controllers;

public class AppController : Controller
{
    private readonly MainContext _context;

    public AppController(MainContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Index()
    {
        var index = new anuR.Views.App.Index
        {
            Users = _context.Users.Include(s => s.Services).Include(s => s.Sites).ToList(),
            Sites = _context.Sites.Include(s => s.Users).ToList(),
            Services = _context.Services.Include(s => s.Users).ToList(),
        };


        return View(index);
    }

    public IActionResult Site()
    {
        return View();
    }

    public IActionResult Service()
    {
        return View();
    }

    public IActionResult User()
    {
        return View();
    }
}