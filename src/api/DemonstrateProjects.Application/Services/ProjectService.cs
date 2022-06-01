using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using DemonstrateProjects.Application.ViewModels;
using DemonstrateProjects.Core.Interfaces;

namespace DemonstrateProjects.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;    
    }

    public Task AddAsync(Guid UserId, NewProjectModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<ProjectModel>> GetFromUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectModel?> GetAsync(Guid userId, int index)
    {
        throw new NotImplementedException();
    }

    public Task EditAsync(Guid userId, int index, EditProjectModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid userId, int index)
    {
        throw new NotImplementedException();
    }
}