using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemonstrateProjects.Api.Controllers;

[Authorize]
[ApiController]
[Route("/keys")]
public class PersonalReadKeyController : ControllerBase
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    private readonly IAuthService _authService;
    private readonly IPersonalReadKeyService _personalReadKeyService;

    public PersonalReadKeyController(UserManager<IdentityUser<Guid>> userManager, IAuthService authService, IPersonalReadKeyService personalReadKeyService)
    {
        _userManager = userManager;

        _authService = authService;
        _personalReadKeyService = personalReadKeyService;
    }

    [HttpPost("new")]
    public async Task<IActionResult> CreateAsync(NewPersonalReadKeyModel model)
    {
        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();

        var key = await _personalReadKeyService.CreateAsync(userId, model);
        return CreatedAtAction(nameof(GetAsync) + "Async", await _personalReadKeyService.GetAsync(key));
    }

    [HttpGet("personal")]
    public async Task<IActionResult> GetAllFromUserAsync()
    {
        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();
        
        var keys = await _personalReadKeyService.GetFromUserAsync(userId);
        return Ok(keys);
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetAsync(string key)
    {
        try
        {
            var userId = await GetUserIdAsyncByTokensAsync();
            if (userId == Guid.Empty)
                return Forbid();
            
            var findedKey = (await _personalReadKeyService.GetFromUserAsync(userId)).Where(x => x.Key == Guid.Parse(key));
            if (findedKey is null)
                return NotFound();
            
            return Ok(findedKey);
        }
        catch
        {
            /* Maybe the Guid could not be parsed! */
            return NotFound();
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync([FromQuery] string key)
    {
        try
        {
            var userId = await GetUserIdAsyncByTokensAsync();
            if (userId == Guid.Empty)
                return Forbid();
            
            var findedKey = (await _personalReadKeyService.GetFromUserAsync(userId)).Where(x => x.Key == Guid.Parse(key));
            if (findedKey is null)
                return NotFound();

            await _personalReadKeyService.DeleteAsync(Guid.Parse(key));
            return NoContent();
        }
        catch
        {
            /* Maybe the Guid could not be parsed! */
            return NotFound(); 
        }
    }

    private async Task<Guid> GetUserIdAsyncByTokensAsync()
    {
        var access = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "d_a").Value;
        var refresh = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "d_r").Value;

        if (string.IsNullOrEmpty(access) || string.IsNullOrEmpty(refresh) || (_authService.GetUsernameInToken(access) != _authService.GetUsernameInToken(refresh)))
            return Guid.Empty;

        var user = await _userManager.FindByNameAsync(_authService.GetUsernameInToken(access));
        if (user is null)
            return Guid.Empty;
        
        return user.Id;
    }
}