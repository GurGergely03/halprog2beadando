using Microsoft.EntityFrameworkCore;

namespace CryptoService.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected CryptoContext _context;
    protected DbSet<TEntity> _dbSet;

    public GenericRepository(CryptoContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }


    public async Task<IEnumerable<TEntity>> GetAllAsync(string[]? includedProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (includedProperties != null)
        {
            foreach (var property in includedProperties)
            {
                query = query.Include(property);
            }
        }
        return await query.ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id, string[]? includedReferences = null, string[]? includedCollections = null)
    {
        TEntity? entity = await _dbSet.FindAsync(id);
        if (entity == null)
        {
            return null;
        }
        
        List<Task> tasks = new List<Task>();

        if (includedReferences != null)
        {
            foreach (var reference in includedReferences)
            {
                tasks.Add(_context.Entry(entity).Reference(reference).LoadAsync());
            }
        }

        if (includedCollections != null)
        {
            foreach (string collection in includedCollections)
            {
                tasks.Add(_context.Entry(entity).Collection(collection).LoadAsync());
            }
        }
        
        await Task.WhenAll(tasks);
        
        return entity;
    }
    
    public async Task InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(int id)
    {
        TEntity? entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
}