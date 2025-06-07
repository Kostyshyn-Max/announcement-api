namespace Announcement.Models.Models;

/// <summary>
/// Represents the details of an announcement, including additional information
/// such as description and similar announcements.
/// </summary>
public record AnnouncementDetailsModel : AnnouncementModel
{
    /// <summary>
    /// Gets or sets the description of the announcement.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a collection of announcements that are similar to the current announcement.
    /// </summary>
    public List<AnnouncementModel> SimilarAnnouncements { get; set; } = new List<AnnouncementModel>();
}