namespace Announcement.BusinessLogic.Tests;
using Announcement = DataAccess.Entities.Announcement;

/// <summary>
/// Provides methods to seed default data for testing purposes.
/// </summary>
public static class DataSeed
{
    /// <summary>
    /// Gets a collection of default announcements for seeding the database.
    /// </summary>
    /// <returns>An enumerable collection of default announcements.</returns>
    public static IEnumerable<Announcement> GetDefaultAnnouncements()
    {
        return new List<Announcement>
        {
            new ()
            {
                Id = 1,
                Title = "test",
                Description = "test desc",
                AddedDate = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            },
            new ()
            {
                Id = 2,
                Title = "Similar Announcement",
                Description = "Description for Announcement similar",
                AddedDate = new DateTime(2024, 1, 2, 11, 0, 0, DateTimeKind.Utc),
            },
            new ()
            {
                Id = 3,
                Title = "Similar Announcement 1",
                Description = "Similar Announcement 1 Description",
                AddedDate = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            },
            new ()
            {
                Id = 4,
                Title = "Similar Announcement 2",
                Description = "Similar Announcement 2 Description",
                AddedDate = new DateTime(2024, 1, 2, 11, 0, 0, DateTimeKind.Utc),
            },
            new ()
            {
                Id = 5,
                Title = "Similar Announcement 3",
                Description = "Similar Announcement 3 Description",
                AddedDate = new DateTime(2024, 1, 3, 12, 0, 0, DateTimeKind.Utc),
            },
            new ()
            {
                Id = 6,
                Title = "Similar Announcement 4",
                Description = "Similar Announcement 4 Description",
                AddedDate = new DateTime(2024, 1, 4, 13, 0, 0, DateTimeKind.Utc),
            },
        };
    }
}