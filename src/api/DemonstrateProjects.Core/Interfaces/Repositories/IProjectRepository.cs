using DemonstrateProjects.Core.Entities;

namespace DemonstrateProjects.Core.Interfaces.Repositories;

public interface IProjectRepository : IBaseRepository<Guid, Project>
{
    Task<IQueryable<Project>> GetByUserIdAsync(Guid userId);
    Task<Project?> GetByUserIdAndIndexAsync(Guid userId, int index);
}