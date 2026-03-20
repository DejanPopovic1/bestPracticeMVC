using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.Exceptions;

namespace ConfigService.Database;

public interface IEntityRepository<T, I> where T : class
{
    /// <summary>
    /// Get a single entity by Id
    /// </summary>
    /// <param name="id">The Id of the entity</param>
    Task<T?> FindByIdAsync(I id);

    /// <summary>
    /// Get a single entity by Id, 
    /// Throwing an <exception cref="EntityNotFoundException">EntityNotFoundException</exception> if not found
    /// </summary>
    /// <param name="id">The Id of the entity</param>
    Task<T> FindByIdOrFailAsync(I id);

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="disableTracking">Disables change tracking for entities returned by this query. Changes to these entities will never persist.</param>
    Task<List<T>> FindAllAsync(bool disableTracking);

    /// <summary>
    /// Gets the first entity that matches a filter predicate
    /// </summary>
    /// <param name="filter">The filter predicate function to specify query conditions</param>
    /// <param name="disableTracking">Disables change tracking for entities returned by this query. Changes to these entities will never persist.</param>
    /// <returns></returns>
    T? FindByCondition(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking);

    /// <summary>
    /// Gets a single entity that matches a filter predicate
    /// </summary>
    /// <param name="filter">The filter predicate function to specify query conditions</param>
    /// <param name="disableTracking">Disables change tracking for entities returned by this query. Changes to these entities will never persist.</param>
    /// <returns></returns>
    Task<T?> FindByConditionAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking);

    /// <summary>
    /// Gets a single entity that matches a filter predicate,
    /// Throwing an <exception cref="EntityNotFoundException">EntityNotFoundException</exception> if not found
    /// </summary>
    /// <param name="filter">The filter predicate function to specify query conditions</param>
    /// <param name="disableTracking">Disables change tracking for entities returned by this query. Changes to these entities will never persist.</param>
    /// <returns></returns>
    Task<T> FindByConditionOrFailAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking);

    /// <summary>
    /// Gets the first entity that matches a filter predicate
    /// </summary>
    /// <param name="filter">The filter predicate function to specify query conditions</param>
    /// <param name="disableTracking">Disables change tracking for entities returned by this query. Changes to these entities will never persist.</param>
    /// <returns></returns>
    Task<T?> FindFirstByConditionAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking);

    /// <summary>
    /// Gets the first entity that matches a filter predicate,
    /// Throwing an <exception cref="EntityNotFoundException">EntityNotFoundException</exception> if not found
    /// </summary>
    /// <param name="filter">The filter predicate function to specify query conditions</param>
    /// <param name="disableTracking">Disables change tracking for entities returned by this query. Changes to these entities will never persist.</param>
    /// <returns></returns>
    Task<T> FindFirstByConditionOrFailAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking);

    /// <summary>
    /// Get all entities that matches a filter predicate
    /// </summary>
    /// <param name="filter">The filter predicate function to specify query conditions</param>
    /// <param name="disableTracking">Disables change tracking for entities returned by this query. Changes to these entities will never persist.</param>
    /// <returns></returns>
    Task<List<T>> FindAllByConditionAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking);

    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="entity">The entity to create</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="entity">The entity to create</param>
    void Add(T entity);

    /// <summary>
    /// Update an entity
    /// </summary>
    /// <param name="entity">The entity to update</param>
    void Update(T entity);

    /// <summary>
    /// Update a range of entities
    /// </summary>
    /// <param name="entities">The entities to update</param>
    void UpdateRange(T[] entities);

    /// <summary>
    /// Persists all changes made to the database 
    /// </summary>
    /// <returns></returns>
    int SaveChanges();

    /// <summary>
    /// Persists all changes made to the database 
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();
}

/// <summary>
/// The base entity repository from which all entity repositories should extend
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="I"></typeparam>
public class EntityRepository<T, I> : IEntityRepository<T, I> where T : class
{
    protected readonly ApplicationContext _context;
    protected readonly DbSet<T> _dbSet;

    public EntityRepository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> FindByIdAsync(I id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> FindByIdOrFailAsync(I id)
    {
        var e = await FindByIdAsync(id);
        return e ?? throw new EntityNotFoundException(typeof(T), new { Id = id });
    }

    public async Task<List<T>> FindAllAsync(bool disableTracking = false)
    {
        return await (disableTracking ? _dbSet.AsNoTracking().ToListAsync() : _dbSet.ToListAsync());
    }

    public T? FindByCondition(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking = false)
    {
        var query = filter(disableTracking ? _dbSet.AsQueryable().AsNoTracking() : _dbSet.AsQueryable());
        return query.SingleOrDefault();
    }

    public async Task<T?> FindByConditionAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking = false)
    {
        var query = filter(disableTracking ? _dbSet.AsQueryable().AsNoTracking() : _dbSet.AsQueryable());
        return await query.SingleOrDefaultAsync();
    }

    public async Task<T> FindByConditionOrFailAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking = false)
    {
        return await FindByConditionAsync(filter, disableTracking) ?? throw new EntityNotFoundException(typeof(T), new { Filter = filter });
    }

    public async Task<T?> FindFirstByConditionAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking = false)
    {
        var query = filter(disableTracking ? _dbSet.AsQueryable().AsNoTracking() : _dbSet.AsQueryable());
        return await query.FirstOrDefaultAsync();
    }

    public async Task<T> FindFirstByConditionOrFailAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking = false)
    {
        return await FindFirstByConditionAsync(filter, disableTracking) ?? throw new EntityNotFoundException(typeof(T), new { Filter = filter });
    }

    public async Task<List<T>> FindAllByConditionAsync(Func<IQueryable<T>, IQueryable<T>> filter, bool disableTracking = false)
    {
        var query = filter(disableTracking ? _dbSet.AsQueryable().AsNoTracking() : _dbSet.AsQueryable());
        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(T[] entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}