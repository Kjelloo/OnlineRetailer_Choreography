using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using UserApi.Core.Models;
using UserApi.Core.Service;

namespace UserApi.Domain.Service;

public class AuthService : IAuthService
{
    private readonly byte[] _secret;

    public AuthService(byte[] secret)
    {
        _secret = secret;
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, User user)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Surname, user.CustomerId.ToString())
        };
        
        var token = new JwtSecurityToken(
            new JwtHeader(new SigningCredentials(
                new SymmetricSecurityKey(_secret),
                SecurityAlgorithms.HmacSha256)),
            new JwtPayload(null,
                null,
                claims.ToArray(),
                DateTime.Now,
                DateTime.Now.AddMinutes(10)));
            
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}