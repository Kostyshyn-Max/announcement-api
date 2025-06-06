namespace Announcement.DataAccess.Repositories;

using Announcement.DataAccess.EF;

/// <summary>
/// Provides a base repository with access to the <see cref="ApplicationDbContext"/>.
/// </summary>
public abstract class AbstractRepository
{
    /// <summary>
    /// The database context used by the repository.
    /// </summary>
    protected readonly ApplicationDbContext сontext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractRepository"/> class with the specified context.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <exception cref="ArgumentNullException">Throws an exception when context is null.</exception>
    protected AbstractRepository(ApplicationDbContext context)
    {
        this.сontext = context ?? throw new ArgumentNullException(nameof(context));
    }
}