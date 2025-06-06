namespace Announcement.BusinessLogic.Tests;

using Announcement.DataAccess.EF;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Factory class for creating and managing an instance of <see cref="ApplicationDbContext"/>
/// for integration tests.
/// </summary>
public static class AnnouncementDbContextFactory
{
    /// <summary>
    /// The connection string used to connect to the test database.
    /// </summary>
    private static readonly string _connectionString =
        "Host=localhost;Port=5432;Database=announcement_tests_db;Username=postgres;Password=postgres";

    /// <summary>
    /// Singleton instance of the <see cref="ApplicationDbContext"/>.
    /// </summary>
    private static ApplicationDbContext? _instance;

    /// <summary>
    /// Gets the singleton instance of the <see cref="ApplicationDbContext"/>.
    /// Ensures the database is deleted and migrated before returning the instance.
    /// </summary>
    public static ApplicationDbContext Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new ApplicationDbContext(CreateOptions());
            _instance.Database.EnsureDeleted();
            _instance.Database.Migrate();

            return _instance;
        }
    }

    /// <summary>
    /// Resets the singleton instance of the <see cref="ApplicationDbContext"/> 
    /// by ensuring the database is deleted and setting the instance to null.
    /// </summary>
    public static void Reset()
    {
        _instance?.Database.EnsureDeleted();
        _instance = null;
    }

    /// <summary>
    /// Resets the singleton instance of the <see cref="ApplicationDbContext"/> 
    /// by ensuring the database is deleted and setting the instance to null.
    /// </summary>
    public static void Seed()
    {
        var context = Instance;

        if (!context.Announcements.Any())
        {
            context.Announcements.AddRange(DataSeed.GetDefaultAnnouncements());
            context.SaveChanges();
        }
    }

    /// <summary>
    /// Creates the options for configuring the <see cref="ApplicationDbContext"/>
    /// using the connection string.
    /// </summary>
    /// <returns>The configured <see cref="DbContextOptions{TContext}"/>.</returns>
    private static DbContextOptions<ApplicationDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_connectionString)
            .Options;
    }
}