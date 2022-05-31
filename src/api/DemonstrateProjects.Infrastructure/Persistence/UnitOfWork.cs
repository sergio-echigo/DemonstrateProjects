using DemonstrateProjects.Core.Interfaces;
using DemonstrateProjects.Core.Interfaces.Repositories;
using DemonstrateProjects.Infrastructure.Persistence.Repositories;

namespace DemonstrateProjects.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private readonly IProjectRepository _projects;
    private readonly IPersonalReadKeyRepository _personalReadKeys;

    public IProjectRepository Projects => throw new NotImplementedException();
    public IPersonalReadKeyRepository PersonalReadKeys => throw new NotImplementedException();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        _projects ??= new ProjectRepository(context);
        _personalReadKeys ??= new PersonalReadKeyRepository(context);
    }

    public async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}