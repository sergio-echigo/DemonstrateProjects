using DemonstrateProjects.Core.Interfaces.Repositories;

namespace DemonstrateProjects.Infrastructure.Persistence.Repositories;

public class BaseRepository<TKey, TEntity> : IBaseRepository<TKey, TEntity> where TEntity : class
{
    private readonly AppDbContext _context;
    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task CreateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<TEntity>> GetEntitiesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<TEntity>> GetByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> GetEntityAsync(TKey key)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TKey key, TEntity updatedEntity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TKey key)
    {
        throw new NotImplementedException();
    }
}