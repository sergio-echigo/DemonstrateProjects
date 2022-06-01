namespace DemonstrateProjects.Core.Interfaces.Repositories;

public interface IBaseRepository<TKey, TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);

    Task<IQueryable<TEntity>> GetEntitiesAsync();
    Task<TEntity?> GetEntityAsync(TKey key);

    Task UpdateAsync(TKey key, TEntity updatedEntity);
    Task DeleteAsync(TKey key);
}