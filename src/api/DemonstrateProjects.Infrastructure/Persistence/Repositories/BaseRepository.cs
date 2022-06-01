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

    public async Task Add(TEntity entity)
    {
        _dbSet.Add(entity);
        await Task.CompletedTask;
    }

    public async Task<IQueryable<TEntity>> GetEntitiesAsync()
    {
        return await Task.FromResult(_dbSet.AsQueryable());
    }

    public async Task<TEntity?> GetEntityAsync(TKey key)
    {
        return await _dbSet.FindAsync(key);
    }

    public async Task UpdateAsync(TEntity updatedEntity)
    {
        _dbSet.Update(updatedEntity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(TKey key)
    {
        var toDelete = await _dbSet.FindAsync(key);

        /* We're going to verify if entity exists on controllers! */
        _dbSet.Remove(toDelete!);
    }
}