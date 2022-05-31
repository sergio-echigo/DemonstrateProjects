using DemonstrateProjects.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DemonstrateProjects.Infrastructure.Persistence.Repositories;

public class BaseRepository<TKey, TEntity> : IBaseRepository<TKey, TEntity> where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
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