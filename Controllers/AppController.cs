using anuR.Context;
using anuR.Models;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
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

    [HttpGet]
    public IActionResult Site(int Id)
    {
        Site site = _context.Sites.Include(s => s.Users).FirstOrDefault(s => s.Id == Id);
        if (site == null) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        return View(site);
    }
    
    [HttpGet]
    public IActionResult Sites()
    {
        List<Site> sites = _context.Sites.Include(s => s.Users).ToList();
        return View(sites);
    }

    [HttpGet]
    public IActionResult Service(int Id)
    {
        Service service =  _context.Services.Include(s => s.Users).FirstOrDefault(s => s.Id == Id);
        ViewBag.Services = _context.Services.ToList();
        if (service == null) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        return View(service);
    }
    
    [HttpGet]
    public IActionResult Services()
    {
        List<Service> services = _context.Services.Include(s => s.Users).ToList();
        return View(services);
    }
    
    [HttpGet]
    public IActionResult User(Guid id)
    {
        var user = _context.Users.Include(s => s.Services).Include(s => s.Sites).FirstOrDefault(u => u.Id == id);
        if (user == null) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        
        ViewBag.Sites = _context.Sites.ToList();
        ViewBag.Services = _context.Services.ToList();
        return View(user);
    }
    
    [HttpGet]
    public IActionResult Users()
    {
        List<User> users = _context.Users.Include(s => s.Services).Include(s => s.Sites).ToList(); 
        return View(users);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveSite(Guid userId, int siteId)
    {
        var user = _context.Users
            .Include(s => s.Sites)
            .FirstOrDefault(u => u.Id == userId);

        var site = _context.Sites.FirstOrDefault(s => s.Id == siteId);

        if (user == null || site == null) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        if (!user.Sites.Contains(site)) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        user.Sites.Remove(site);
        _context.SaveChanges();
        return Redirect(Request.Headers["Referer"].ToString());

    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AddSite(Guid userId, [FromForm] int siteId)
    {
        var user = _context.Users
            .Include(s => s.Sites)
            .FirstOrDefault(u => u.Id == userId);

        var site = _context.Sites.FirstOrDefault(s => s.Id == siteId);

        if (user == null || site == null) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        user.Sites.Add(site);
        _context.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());

    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoveService(Guid userId, int serviceId)
    {
        var user = _context.Users.Include(s => s.Services)
            .FirstOrDefault(u => u.Id == userId);

        var service = _context.Services.FirstOrDefault(s => s.Id == serviceId);

        if (user == null || service == null) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        if (!user.Services.Contains(service)) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        
        user.Services.Remove(service);
        _context.SaveChanges();
        
        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AddService(Guid userId, [FromForm] int siteId)
    {
        var user = _context.Users.Include(s => s.Services)
            .FirstOrDefault(u => u.Id == userId);

        var service = _context.Services.FirstOrDefault(s => s.Id == siteId);

        if (user == null || service == null) return RedirectToAction("NotFound", "Error"); // TODO: 404 page
        user.Services.Add(service);
        _context.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());

    }
}