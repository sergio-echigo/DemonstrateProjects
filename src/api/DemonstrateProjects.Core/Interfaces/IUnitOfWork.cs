using DemonstrateProjects.Core.Interfaces.Repositories;

namespace DemonstrateProjects.Core.Interfaces;

public interface IUnitOfWork
{
    IProjectRepository Projects { get; }
    IPersonalReadKeyRepository PersonalReaKeys { get; }

    Task SaveChangesAsync();
}