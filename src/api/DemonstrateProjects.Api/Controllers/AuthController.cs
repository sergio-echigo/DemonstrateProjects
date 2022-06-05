using System.IdentityModel.Tokens.Jwt;
using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemonstrateProjects.Api.Controllers;

[ApiController]
[Route("/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<IdentityUser<Guid>> usermanager, IAuthService authService)
    {
        _userManager = usermanager;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterUserModel model)
    {
        var existentByName = await _userManager.FindByNameAsync(model.Username);
        var existentByEmail = await _userManager.FindByEmailAsync(model.Email);

        if (!(existentByName is null && existentByEmail is null))
            return BadRequest();
        
        var newUser = new IdentityUser<Guid>()
        {
            Id = Guid.NewGuid(),
            UserName = model.Username,
            Email = model.Email,
            PasswordHash = _authService.GeneratePasswordHash(model.Password)
        };

        return CreatedAtAction(nameof(RegisterAsync) + "Async", model);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserModel model)
    {
        var existentByName = await _userManager.FindByNameAsync(model.Main);
        var existentByEmail = await _userManager.FindByEmailAsync(model.Main);

        if (existentByName is null && existentByEmail is null)
            return BadRequest();

        /* At least one of these isn't null */
        if (existentByName is not null)
        {
            if (_authService.PasswordHashAreEqual(model.Password, existentByName.PasswordHash))
            {
                SetAuthCookies(_authService.GenerateToken(existentByName.UserName, false), _authService.GenerateToken(existentByName.UserName, true));
                return Ok();
            }
            else
                return BadRequest();
        }
        else
        {
            if (_authService.PasswordHashAreEqual(model.Password, existentByEmail.PasswordHash))
            {
                SetAuthCookies(_authService.GenerateToken(existentByEmail.UserName, false), _authService.GenerateToken(existentByEmail.UserName, true));
                return Ok();
            }
            else
                return BadRequest();
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        var access = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "d_a");
        if (access.Key is null || access.Value is null)
            return BadRequest();

        var token = new JwtSecurityToken(access.Value);
        if (token.ValidTo > DateTimeOffset.Now)
            return BadRequest();
        
        var username = _authService.GetUsernameInToken(access.Value);
        if (username is null)
            return BadRequest();

        var newAccess = _authService.GenerateToken(username, false);
        var newRefresh = _authService.GenerateToken(username, true);

        SetAuthCookies(newAccess, newRefresh);
        
        await Task.CompletedTask;
        return Ok();
    }

    private void SetAuthCookies(string access, string refresh)
    {
        HttpContext.Response.Cookies.Delete("d_a");
        HttpContext.Response.Cookies.Delete("d_r");

        /* "Secure = False" only in development mode! */
        HttpContext.Response.Cookies.Append("d_a", access, new CookieOptions() { Expires = DateTimeOffset.MinValue });
        HttpContext.Response.Cookies.Append("d_r", refresh, new CookieOptions() { Expires = DateTimeOffset.MinValue, HttpOnly = true });
    }
}