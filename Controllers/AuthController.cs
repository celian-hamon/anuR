using System.Security.Claims;
using System.Text.Encodings.Web;
using anuR.Context;
using anuR.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using anuR.Services;

namespace anuR.Controllers;

public class AuthController : Controller
{
    //controller d'authentification avec le register et le login
    private readonly MainContext _context;
    private readonly IHashService _hashService;

    public AuthController(MainContext context, IHashService hashService)
    {
        _context = context;
        _hashService = hashService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login login)
    {
        if (!ModelState.IsValid) return View(login);
        User? user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == login.Email && u.Password == _hashService.GetHash(login.Password));
        if (user == null) return RedirectToAction("Login", "Auth");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(claimsIdentity);
        var props = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
        };

        HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            props).Wait();

        return RedirectToAction("Index", "App");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Register register)
    {
        if (!ModelState.IsValid) return View(register);
        User user = new User
        {
            Email = register.Email,
            Password = _hashService.GetHash(register.Password),
            IsAdmin = false,
            FirstName = register.FirstName,
            LastName = register.LastName,
            PhoneNumber = register.PhoneNumber,
            LandLine = register.LandLine
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}