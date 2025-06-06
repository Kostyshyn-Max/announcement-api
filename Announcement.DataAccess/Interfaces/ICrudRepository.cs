namespace Announcement.DataAccess.Interfaces;

using System.Linq.Expressions;

/// <summary>
/// Generic interface for CRUD (Create, Read, Update, Delete) operations on a repository.
/// </summary>
/// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
public interface ICrudRepository<T>
{
    /// <summary>
    /// Creates a new entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>The ID of the created entity.</returns>
    Task<int> CreateAsync(T entity);

    /// <summary>
    /// Retrieves all entities from the repository.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves a paginated list of entities from the repository.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of entities per page.</param>
    /// <returns>A collection of entities for the specified page.</returns>
    Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);

    /// <summary>
    /// Retrieves entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to filter entities.</param>
    /// <returns>A collection of entities that match the predicate.</returns>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The entity containing the ID to search for.</param>
    /// <returns>The entity with the specified ID, or null if not found.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(int id);
}