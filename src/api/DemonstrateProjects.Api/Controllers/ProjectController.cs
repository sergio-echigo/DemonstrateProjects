using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemonstrateProjects.Api.Controllers;

[Authorize]
[ApiController]
[Route("/projects")]
public class ProjectController : ControllerBase
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly IAuthService _authService;

    private readonly IProjectService _projectService;
    private readonly IPersonalReadKeyService _personalReadKeyService;

    public ProjectController(UserManager<IdentityUser<Guid>> userManager, 
                             IAuthService authService, 
                             IProjectService projectService, 
                             IPersonalReadKeyService personalReadKeyService)
    {
        _userManager = userManager;
        _authService = authService;

        _projectService = projectService;
        _personalReadKeyService = personalReadKeyService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateNewAsync([FromBody] NewProjectModel model)
    {
        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();
            
        var created = await _projectService.AddAsync(userId, model);

        return CreatedAtAction(nameof(GetPersonalProjectAsync) + "Async", created);
    }

    [HttpGet("{index}")]
    public async Task<IActionResult> GetPersonalProjectAsync([FromRoute] int index)
    {
        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();

        var project = await _projectService.GetAsync(userId, index);
        if (project is null)
            return NotFound();

        return Ok(project);
    }

    [HttpGet("key")]
    public async Task<IActionResult> GetProjectsByReadKey([FromBody] ReadKeyModel model, [FromQuery] int? index)
    {
        var personalReadKey = await _personalReadKeyService.GetAsync(Guid.Parse(model.Key));
        if (personalReadKey is null)
            return Forbid();
        
        if (personalReadKey.ExpiresWhen <= DateTimeOffset.Now)
        {
            await _personalReadKeyService.DeleteAsync(personalReadKey.Key);
            return BadRequest();
        }

        var projects = await _projectService.GetFromUserAsync(personalReadKey.UserId);

        if (index is null)
            return Ok(projects);
        else
            if (projects.Any(x => x.Index == index))
                return Ok(projects.SingleOrDefault(x => x.Index == index));
            else
                return NotFound();
    }

    [HttpPut("{index}/edit")]
    public async Task<IActionResult> EditAsync([FromRoute] int index, [FromBody] EditProjectModel model)
    {
        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();

        if (!(await _projectService.GetFromUserAsync(userId)).Any(x => x.Index == index))
            return NotFound();
        else
            await _projectService.EditAsync(userId, index, model);
        
        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync([FromQuery] int index)
    {
        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();

        if (!(await _projectService.GetFromUserAsync(userId)).Any(x => x.Index == index))
            return NotFound();
        else
            await _projectService.DeleteAsync(userId, index);
        
        return NoContent();
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