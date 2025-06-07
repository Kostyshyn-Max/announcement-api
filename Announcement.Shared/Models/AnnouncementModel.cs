namespace Announcement.Models.Models;

/// <summary>
/// Represents a model for an announcement in the business logic layer.
/// </summary>
public record AnnouncementModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the announcement.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the announcement.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the announcement was added.
    /// </summary>
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
}