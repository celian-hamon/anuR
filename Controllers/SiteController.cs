using anuR.Context;
using anuR.Models;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Controllers;

public class SiteController : Controller
{
    private readonly MainContext _context;
    
    public SiteController(MainContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Site site)
    {
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Add(site);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Site site)
    {
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Update(site);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        Site? site = _context.Sites.FirstOrDefault(s => s.Id == Id);
        if (site == null) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Remove(site);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());;
    }
}