namespace DemonstrateProjects.Application.Services.Interfaces;

public interface IAuthService
{
    string GenerateToken(string username, bool isRefresh);
    string? GetUsernameInToken(string token);

    string GeneratePasswordHash(string pswd);
    bool PasswordHashAreEqual(string pswd, string pswdHash);
}