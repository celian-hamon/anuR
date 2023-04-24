using anuR.Context;
using anuR.Models;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Controllers;

public class ServiceController : Controller
{
    private MainContext _context;
    
    public ServiceController(MainContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Service service)
    {
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Add(service);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Service service)
    {
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Update(service);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        Service? service = _context.Services.FirstOrDefault(s => s.Id == Id);
        if (service == null) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Remove(service);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }
}