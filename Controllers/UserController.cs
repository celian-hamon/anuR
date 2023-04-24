using anuR.Context;
using anuR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly MainContext _context;

    public UserController(MainContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(User user) //TODO Model validation
    {
        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Users", "App");
    }

    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    [Route("Edit/{Id}")]
    public async Task<IActionResult> Edit(User user)
    {
        //TODO check if user is self or admin
        if (!ModelState.IsValid) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Update(user);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("Delete/{Id}")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        User? user = _context.Users.FirstOrDefault(s => s.Id == Id);
        if (user == null) return Redirect(Request.Headers["Referer"].ToString());;
        _context.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Users", "App");
    }
}