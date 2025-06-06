namespace Announcement.Models.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a model for updating an announcement in the business logic layer.
/// </summary>
public class AnnouncementUpdateModel
{
    /// <summary>
    /// Gets or sets the title of the announcement.
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the announcement.
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;
}