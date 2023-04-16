using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using anuR.Context;
using anuR.Models;
using anuR.Services.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using stocknet_api_v2.Services;

namespace anuR.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _config;
    private readonly MainContext _context;
    private readonly IHashService _hashService;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(ILogger<AuthenticationService> logger, MainContext context,
        IConfiguration config, IHashService hashService)
    {
        _logger = logger;
        _context = context;
        _config = config;
        _hashService = hashService;
    }

    public ActionResult<Token> Login(Login login)
    {
        var user = Authenticate(login.Email, login.Password);
        if (user == null)
            return new UnauthorizedResult();

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
        };

        var accessToken = GenerateToken(_config["Jwt:Key"]!, claims);
        var refreshToken = GenerateRefreshToken();

        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToInt32(_config["Jwt:Expiry"]));
        user.RefreshToken = refreshToken;

        _context.SaveChanges();

        return new Token
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public  ActionResult<Token> RefreshToken(Token token)
    {
        string accessToken = token.AccessToken!;
        string refreshToken = token.RefreshToken!;
        var principal = GetPrincipalFromExpiredToken(accessToken);
        var mail = principal.FindFirst(ClaimTypes.Email)!.Value;
        var user = _context.Users.FirstOrDefault(u => u.Email == mail);
        if (user.RefreshToken != refreshToken)
            return new BadRequestResult();
        var newAccessToken = GenerateToken(_config["Jwt:Key"]!, principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        _context.Users.Update(user);
        _context.SaveChanges();
        return new Token
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public Models.User? Authenticate(string email, string password)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Email == email && u.Password == _hashService.GetHash(password));

        if (user == null) return null;
        return _hashService.VerifyHash(password, user.Password) ? user : null;
    }

    public string GenerateToken(string secret, List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:Expiry"])),
            SigningCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _config["Jwt:Issuer"],
            ValidAudience = _config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
}