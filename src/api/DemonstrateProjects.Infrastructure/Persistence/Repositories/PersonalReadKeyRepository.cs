using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DemonstrateProjects.Infrastructure.Persistence.Repositories;

public class PersonalReadKeyRepository : BaseRepository<Guid, PersonalReadKey>, IPersonalReadKeyRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<PersonalReadKey> _dbSet;
    
    public PersonalReadKeyRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<PersonalReadKey>();
    }

    public async Task<IQueryable<PersonalReadKey>> GetByUserIdAsync(Guid userId) =>
        await Task.FromResult(_dbSet.AsNoTracking().AsQueryable().Where(x => x.UserId == userId));
}