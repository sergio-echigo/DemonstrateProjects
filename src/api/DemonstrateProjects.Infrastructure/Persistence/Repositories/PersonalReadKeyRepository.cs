using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces.Repositories;

namespace DemonstrateProjects.Infrastructure.Persistence.Repositories;

public class PersonalReadKeyRepository : BaseRepository<Guid, PersonalReadKey>, IPersonalReadKeyRepository
{
    public PersonalReadKeyRepository(AppDbContext context) : base(context)
    {
        
    }
}