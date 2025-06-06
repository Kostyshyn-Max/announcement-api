namespace Announcement.DataAccess.EF;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the application's database context, providing access to the database entities.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to configure the database context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the database set for announcements, representing the <see cref="Entities.Announcement"/> table.
    /// </summary>
    public DbSet<Entities.Announcement> Announcements { get; set; }
}