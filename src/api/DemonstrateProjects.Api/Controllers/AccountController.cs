using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using DemonstrateProjects.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemonstrateProjects.Api.Controllers;

[Authorize]
[ApiController]
[Route("/account")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthService _authService;

    private readonly IProjectService _projectService;
    private readonly IPersonalReadKeyService _personalReadKeyService;

    public AccountController(UserManager<AppUser> userManager, 
                             IAuthService authService, 
                             IProjectService projectService, 
                             IPersonalReadKeyService personalReadKeyService)
    {
        _userManager = userManager;
        _authService = authService;

        _projectService = projectService;
        _personalReadKeyService = personalReadKeyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var id = await GetUserIdAsyncByTokenAsync();
        if (id == Guid.Empty)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return NotFound();

        var projects = (await _projectService.GetFromUserAsync(id)).Count();
        var keys = (await _personalReadKeyService.GetFromUserAsync(id)).Count();

        return Ok(new 
        {
            username = user.UserName,
            email = user.Email,
            activeProjects = projects,
            activeKeys = keys
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromQuery] string pswd)
    {
        var id = await GetUserIdAsyncByTokenAsync();
        if (id == Guid.Empty)
            return Unauthorized();
        
        var apparentlyUser = await _userManager.FindByIdAsync(id.ToString());

        if (!_authService.PasswordHashAreEqual(pswd, apparentlyUser.PasswordHash))
            return Unauthorized();

        await _projectService.DeleteAllAsync(id);
        await _personalReadKeyService.DeleteAllAsync(id);

        await _userManager.DeleteAsync(apparentlyUser);

        HttpContext.Response.Cookies.Delete("d_a", new CookieOptions() { Secure = true, SameSite = SameSiteMode.None });
        HttpContext.Response.Cookies.Delete("d_r", new CookieOptions() { Secure = true, SameSite = SameSiteMode.None, HttpOnly = true });

        return Ok();
    }

    private async Task<Guid> GetUserIdAsyncByTokenAsync()
    {
        var access = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "d_a").Value;
        if (string.IsNullOrEmpty(access) || (_authService.GetUsernameInToken(access) is null))
            return Guid.Empty;

        var user = await _userManager.FindByNameAsync(_authService.GetUsernameInToken(access));
        if (user is null)
            return Guid.Empty;
        
        return user.Id;
    }
}