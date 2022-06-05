using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        try
        {
            var parameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidAudience = _config.GetSection("JwtConfig:Audience").Value,
                ValidIssuer = _config.GetSection("JwtConfig:Issuer").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtConfig:SecretSecureToken").Value))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token!");

            return principal.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
        }
        catch
        {
            return null;
        }
    }

    public string GeneratePasswordHash(string pswd)
    {
        byte[] salt;
        byte[] hash;

        byte[] hashBytes = new byte[36];

        salt = RandomNumberGenerator.GetBytes(16);
        hash = new Rfc2898DeriveBytes(pswd, salt, 50000).GetBytes(20);

        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);
    }

    public bool PasswordHashAreEqual(string pswd, string pswdHash)
    {
        byte[] salt = new byte[16];
        byte[] hash;

        byte[] hashBytes = Convert.FromBase64String(pswdHash);

        Array.Copy(hashBytes, 0, salt, 0, 16);
        hash = new Rfc2898DeriveBytes(pswd, salt, 50000).GetBytes(20);

        for(int i = 0; i < 20; i++)
        {
            if (hashBytes[i+16] != hash[i])
                return false;
        }

        return true;
    }
}