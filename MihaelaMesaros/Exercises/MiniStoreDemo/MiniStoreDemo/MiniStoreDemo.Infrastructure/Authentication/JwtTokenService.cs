using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniStoreDemo.Application.Abstractions.Authentication;
using MiniStoreDemo.Application.DTOs;
using MiniStoreDemo.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniStoreDemo.Infrastructure.Authentication;

public sealed class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public LoginResponseDto GenerateToken(User user)
    {
        var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
        var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
        var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");

        var expiresAt = DateTime.UtcNow.AddMinutes(30);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresAt = expiresAt
        };
    }
}