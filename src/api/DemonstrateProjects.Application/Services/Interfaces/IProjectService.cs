using DemonstrateProjects.Application.ViewModels;
using DemonstrateProjects.Application.InputModels;

namespace DemonstrateProjects.Application.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectModel> AddAsync(Guid UserId, NewProjectModel model);

    Task<IQueryable<ProjectModel>> GetFromUserAsync(Guid userId);
    Task<ProjectModel?> GetAsync(Guid userId, int index);

    Task UploadImgAsync(Guid userId, int index, byte[] img);

    Task EditAsync(Guid userId, int index, EditProjectModel model);
    Task DeleteAsync(Guid userId, int index);

    Task DeleteAllAsync(Guid userId);
}