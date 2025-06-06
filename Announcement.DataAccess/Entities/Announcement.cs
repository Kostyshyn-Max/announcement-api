namespace Announcement.DataAccess.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Represents an announcement entity in the database.
/// </summary>
[Table("announcement")]
public class Announcement
{
    /// <summary>
    /// Gets or sets the unique identifier for the announcement.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the announcement.
    /// </summary>
    [Column("title")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the announcement.
    /// </summary>
    [Column("description")]
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the announcement was added.
    /// </summary>
    [Column("added_date")]
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
}