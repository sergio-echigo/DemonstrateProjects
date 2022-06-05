using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemonstrateProjects.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DemonstrateProjects.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    public AuthService(IConfiguration config)
    {
        _config = config;    
    }

    public string GenerateToken(string username, bool isRefresh)
    {
        var handler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_config.GetSection("JwtConfig:SecretSecureToken").Value);

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("username", username)
            }),
            Expires = (!isRefresh) ? DateTime.UtcNow.AddHours(2) : DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
            Audience = _config.GetSection("JwtConfig:Audience").Value,
            Issuer = _config.GetSection("JwtConfig:Issuer").Value
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);    
    }

    public string? GetUsernameInToken(string token)
    {
        var parameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtConfig:SecretSecureToken").Value))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid Token!");

        return principal.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
    }
}