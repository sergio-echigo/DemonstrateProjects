namespace DemonstrateProjects.Application.Services.Interfaces;

public interface IAuthService
{
    string GenerateToken(string username, bool isRefresh);
    string? GetUsernameInToken(string token);
}