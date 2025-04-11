namespace CryptoService.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(string[]? includedProperties = null);
    Task<TEntity> GetByIdAsync(int id, string[]? includedReferences = null, string[]? includedCollections = null);
    Task InsertAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteByIdAsync(int id);
}