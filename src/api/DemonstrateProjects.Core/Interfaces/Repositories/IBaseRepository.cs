namespace DemonstrateProjects.Core.Interfaces.Repositories;

public interface IBaseRepository<TKey, TEntity> where TEntity : class
{
    Task Add(TEntity entity);

    Task<IQueryable<TEntity>> GetEntitiesAsync();
    Task<TEntity?> GetEntityAsync(TKey key);

    Task UpdateAsync(TEntity updatedEntity);
    Task DeleteAsync(TKey key);
}