using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DemonstrateProjects.Infrastructure.Persistence.Repositories;

public class ProjectRepository : BaseRepository<int, Project>, IProjectRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Project> _dbSet;
    
    public ProjectRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Project>();
    }

    public async Task<IQueryable<Project>> GetByUserIdAsync(Guid userId) =>
        await Task.FromResult(_dbSet.AsQueryable().Where(x => x.UserId == userId));
}