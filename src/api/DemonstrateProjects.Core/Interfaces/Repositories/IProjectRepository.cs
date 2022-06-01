using DemonstrateProjects.Core.Entities;

namespace DemonstrateProjects.Core.Interfaces.Repositories;

public interface IProjectRepository : IBaseRepository<int, Project>
{
    Task<IQueryable<Project>> GetByUserIdAsync(Guid userId);
}