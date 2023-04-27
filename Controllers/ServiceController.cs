using anuR.Context;
using anuR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace anuR.Controllers;

public class ServiceController : Controller
{
    //Controller du CRUD Service
    private MainContext _context;
    
    public ServiceController(MainContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Service service)
    {
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Add(service);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Service service)
    {
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Update(service);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int Id)
    {
        Service? service = _context.Services
            .Include(s => s.Users)
            .FirstOrDefault(s => s.Id == Id);
        if (service == null || service.Users.Count != 0) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Remove(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Services","App");
    }
}