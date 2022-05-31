using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces.Repositories;

namespace DemonstrateProjects.Infrastructure.Persistence.Repositories;

public class ProjectRepository : BaseRepository<int, Project>, IProjectRepository
{
    public ProjectRepository(AppDbContext context) : base(context)
    {
        
    }
}