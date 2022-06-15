using System.IdentityModel.Tokens.Jwt;
using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using DemonstrateProjects.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemonstrateProjects.Api.Controllers;

[ApiController]
[Route("/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<AppUser> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserModel model)
    {
        var existentByName = await _userManager.FindByNameAsync(model.Username);
        var existentByEmail = await _userManager.FindByEmailAsync(model.Email);

        if (!(existentByName is null && existentByEmail is null))
            return BadRequest();
        
        var newUser = new AppUser()
        {
            Id = Guid.NewGuid(),
            UserName = model.Username,
            Email = model.Email,
            PasswordHash = _authService.GeneratePasswordHash(model.Password)
        };

        var success = (await _userManager.CreateAsync(newUser)).Succeeded;
        if (success)
            return CreatedAtAction(nameof(RegisterAsync), model);
    
        return BadRequest();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserModel model)
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

                ImplementTokensCookies(_authService.GenerateToken(existentByName.UserName, false), _authService.GenerateToken(existentByName.UserName, true));
                return Ok();
            }
        }
        
        if (existentByEmail is not null)
        {
            if (_authService.PasswordHashAreEqual(model.Password, existentByEmail.PasswordHash))
            {
                ImplementTokensCookies(_authService.GenerateToken(existentByEmail.UserName, false), _authService.GenerateToken(existentByEmail.UserName, true));               
                return Ok();
            }
        }

        return BadRequest();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        // If user has not a expired access token, we won't allow its refresh.
        var access = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "d_a");
        if (access.Key is null || access.Value is null)
            return BadRequest();
        
        // If user has not a refresh token, we won't allow its refresh too.
        var refresh = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "d_r");
        if (refresh.Key is null || refresh.Value is null)
            return BadRequest();

        // Verifying if access is expired.
        var token = new JwtSecurityToken(access.Value);
        if (token.ValidTo > DateTimeOffset.Now)
            return BadRequest();
        
        // Getting username in refresh token.
        var username = _authService.GetUsernameInToken(refresh.Value);
        if (username is null)
            return BadRequest();

        var newAccess = _authService.GenerateToken(username, false);
        var newRefresh = _authService.GenerateToken(username, true);

        ImplementTokensCookies(newAccess, newRefresh);

        await Task.CompletedTask;
        return Ok();
    }

    private void ImplementTokensCookies(string a, string r)
    {
        HttpContext.Response.Cookies.Append("d_a", a, new CookieOptions() { Secure = true, SameSite = SameSiteMode.None });
        HttpContext.Response.Cookies.Append("d_r", r, new CookieOptions() { Secure = true, SameSite = SameSiteMode.None, HttpOnly = true });
    }
}