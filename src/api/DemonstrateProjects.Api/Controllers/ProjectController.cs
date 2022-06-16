using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using DemonstrateProjects.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemonstrateProjects.Api.Controllers;

[Authorize]
[ApiController]
[Route("/projects")]
public class ProjectController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthService _authService;

    private readonly IProjectService _projectService;
    private readonly IPersonalReadKeyService _personalReadKeyService;

    public ProjectController(UserManager<AppUser> userManager, 
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

        return CreatedAtAction(nameof(CreateNewAsync), model);
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();


        var listOf = await _projectService.GetFromUserAsync(userId);
        return Ok(listOf);
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

    [HttpPost("{index}/img")]
    public async Task<IActionResult> UploadImgAsync([FromRoute] int index, [FromForm] IFormFile file)
    {
        var allowedExt = new string[] 
        {
            ".gif", ".png", ".jpeg", ".jpg"
        };
        
        for(int a = 0, b = 0; a < allowedExt.Length; a++)
        {
            if (!file.FileName.EndsWith(allowedExt[a]))
                b++;
            else
                break;
            
            if (b == allowedExt.Length) 
                return BadRequest();

        }

        var userId = await GetUserIdAsyncByTokensAsync();
        if (userId == Guid.Empty)
            return Forbid();

        if (!(await _projectService.GetFromUserAsync(userId)).Any(x => x.Index == index))
            return NotFound();

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            
            if (memoryStream.Length < 10000000)
            {
                byte[] img = memoryStream.ToArray();
                await _projectService.UploadImgAsync(userId, index, img);
                Console.WriteLine("uploaded");
            }
            else
            {
                return BadRequest();
            }
        }

        return Ok();
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
        var access = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(access) || (_authService.GetUsernameInToken(access) is null))
            return Guid.Empty;

        var user = await _userManager.FindByNameAsync(_authService.GetUsernameInToken(access));
        if (user is null)
            return Guid.Empty;

        return user.Id;
    }
}