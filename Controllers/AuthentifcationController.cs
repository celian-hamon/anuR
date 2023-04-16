using anuR.Models;
using anuR.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService, IConfiguration config)
    {
        _authenticationService = authenticationService;
        _config = config;
    }

    [HttpPost]
    public ActionResult<Token> Login(Login login)
    {
        return _authenticationService.Login(login);
    }
    
    [HttpPost("refresh")]
    public ActionResult<Token> RefreshToken(Token refreshToken)
    {
        return _authenticationService.RefreshToken(refreshToken);
    }
}