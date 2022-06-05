using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using DemonstrateProjects.Application.ViewModels;
using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces;

namespace DemonstrateProjects.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;    
    }

    public async Task<ProjectModel> AddAsync(Guid userId, NewProjectModel model)
    {
        var lastE = await _unitOfWork.Projects.GetByUserIdAsync(userId);
        Project entity = new()
        {
            UserId = userId,

            // We're verifying if the last is default, so no worry about the nullability of lasE.LastOrDefault()            
            Index = (lastE.LastOrDefault() == default) ? 0 : lastE.LastOrDefault()!.Index + 1,

            Title = model.Title.Trim().Replace(" ", "_"),
            Description = model.Description.Trim().Replace("  ", " ")
        };

        await _unitOfWork.Projects.Add(entity);
        await _unitOfWork.SaveChangesAsync();

        return new ProjectModel()
        {
            Index = entity.Index,
            Title = entity.Title,
            Description = entity.Description
        };
    }

    public async Task<IQueryable<ProjectModel>> GetFromUserAsync(Guid userId)
    {
        return (await _unitOfWork.Projects.GetByUserIdAsync(userId))
            .Select(x => new ProjectModel()
            {
                Index = x.Index,
                Title = x.Title,
                Description = x.Description
            });
    }

    public async Task<ProjectModel?> GetAsync(Guid userId, int index)
    {
        var project = await _unitOfWork.Projects.GetByUserIdAndIndexAsync(userId, index);
        
        /* Not required, but only for removing some warnings! And I'm tired of writing "We're verifying..." */
        if (project is null)
            return null;

        return new ProjectModel()
        {
            Index = project.Index,
            Title = project.Title,
            Description = project.Description
        };
    }

    public async Task EditAsync(Guid userId, int index, EditProjectModel model)
    {
        var project = await _unitOfWork.Projects.GetByUserIdAndIndexAsync(userId, index);
        
        Project updated = new()
        {
            Id = project!.Id,
            Index = project.Index,
            UserId = project.UserId,
            Title = model.Title.Trim().Replace(" ", "_"),
            Description = model.Description.Trim().Replace("  ", " ")
        };

        await _unitOfWork.Projects.UpdateAsync(updated);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId, int index)
    {
        var project = await _unitOfWork.Projects.GetByUserIdAndIndexAsync(userId, index);

        await _unitOfWork.Projects.DeleteAsync(project!.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}