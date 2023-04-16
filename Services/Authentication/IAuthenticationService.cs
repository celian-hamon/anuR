using System.Security.Claims;
using anuR.Models;
using Microsoft.AspNetCore.Mvc;

namespace anuR.Services.Authentication;

public interface IAuthenticationService
{
    Models.User? Authenticate(string email, string password);
    string GenerateToken(string secure, List<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    ActionResult<Token> Login (Login login);
    ActionResult<Token> RefreshToken (Token token);
}