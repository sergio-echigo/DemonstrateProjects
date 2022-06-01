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

    public async Task CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        await Task.CompletedTask;
    }

    public async Task<IQueryable<TEntity>> GetEntitiesAsync()
    {
        return await Task.FromResult(_dbSet.AsQueryable());
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