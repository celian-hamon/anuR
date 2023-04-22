using anuR.Context;
using anuR.Models;
using anuR.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace anuR.Controllers;

public class AdminController : Controller
{
    private readonly MainContext _context;

    public AdminController(MainContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var adminViewModel = new anuR.Views.Admin.Index
        {
            Sites = _context.Sites.Take(20).ToList(),
            Services = _context.Services.Take(20).ToList(),
            Users = _context.Users.Take(20).ToList()
        };
        return View(adminViewModel);
    }
}