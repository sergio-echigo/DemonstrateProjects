using DemonstrateProjects.Core.Interfaces;
using DemonstrateProjects.Core.Interfaces.Repositories;

namespace DemonstrateProjects.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    public IProjectRepository Projects => throw new NotImplementedException();
    public IPersonalReadKeyRepository PersonalReadKeys => throw new NotImplementedException();

    public async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}