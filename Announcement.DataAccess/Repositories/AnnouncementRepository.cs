namespace Announcement.DataAccess.Repositories;

using System.Linq.Expressions;
using Announcement.DataAccess.EF;
using Announcement.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Announcement = Entities.Announcement;

/// <summary>
/// Repository for managing announcements in the database.
/// </summary>
public class AnnouncementRepository : AbstractRepository, IAnnouncementRepository
{
    /// <summary>
    /// The database set for announcements, providing access to the <see cref="Entities.Announcement"/> entities.
    /// </summary>
    private readonly DbSet<Announcement> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnnouncementRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="context">The application database context used by repository.</param>
    public AnnouncementRepository(ApplicationDbContext context)
        : base(context)
    {
        this._dbSet = context.Set<Announcement>();
    }

    /// <summary>
    /// Creates a new announcement entity in the database.
    /// </summary>
    /// <param name="entity">The announcement entity to create.</param>
    /// <returns>The ID of the created announcement.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
    public async Task<int> CreateAsync(Announcement entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var entry = await this._dbSet.AddAsync(entity);
        await this.context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    /// <summary>
    /// Retrieves all announcements from the database.
    /// </summary>
    /// <returns>A collection of all announcements.</returns>
    public async Task<IEnumerable<Announcement>> GetAllAsync()
    {
        return await this._dbSet.ToListAsync();
    }

    /// <summary>
    /// Retrieves a paginated list of announcements from the database.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of announcements per page.</param>
    /// <returns>A collection of announcements for the specified page.</returns>
    public async Task<IEnumerable<Announcement>> GetAllAsync(int page, int pageSize)
    {
        var announcements = await this._dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return announcements;
    }

    /// <summary>
    /// Retrieves announcements that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to filter announcements.</param>
    /// <returns>A collection of announcements that match the predicate.</returns>
    public async Task<IEnumerable<Announcement>> GetAllAsync(Expression<Func<Announcement, bool>> predicate)
    {
        var announcements = await this._dbSet.Where(predicate).ToListAsync();
        return announcements;
    }

    /// <summary>
    /// Retrieves an announcement by its ID.
    /// </summary>
    /// <param name="id">The ID to search for.</param>
    /// <returns>The announcement with the specified ID, or null if not found.</returns>
    public async Task<Announcement?> GetByIdAsync(int id)
    {
        var announcement = await this._dbSet.FindAsync(id);
        return announcement;
    }

    /// <summary>
    /// Updates an existing announcement in the database.
    /// </summary>
    /// <param name="entity">The announcement entity to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
    public Task UpdateAsync(Announcement entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        this._dbSet.Update(entity);
        return this.context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an announcement from the database.
    /// </summary>
    /// <param name="entity">The announcement entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
    public Task DeleteAsync(Announcement entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        this._dbSet.Remove(entity);
        return this.context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an announcement from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the announcement to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the announcement with the specified ID is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var announcement = await this._dbSet.FindAsync(id);
        if (announcement is not null)
        {
            this._dbSet.Remove(announcement);
            await this.context.SaveChangesAsync();
        }

        throw new KeyNotFoundException($"Announcement with ID: {id} was not found.");
    }
}