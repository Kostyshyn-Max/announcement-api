namespace Announcement.BusinessLogic.Interfaces;

using Announcement.Models.Models;

/// <summary>
/// Defines the contract for announcement-related business logic operations.
/// </summary>
public interface IAnnouncementService
{
    /// <summary>
    /// Creates a new announcement.
    /// </summary>
    /// <param name="model">The model containing the details of the announcement to create.</param>
    /// <returns>The ID of the created announcement.</returns>
    Task<int> CreateAsync(AnnouncementCreateModel model);

    /// <summary>
    /// Updates an existing announcement with the specified ID using the provided update model.
    /// </summary>
    /// <param name="id">The ID of the announcement to update.</param>
    /// <param name="model">The model containing the updated details of the announcement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(int id, AnnouncementUpdateModel model);

    /// <summary>
    /// Deletes an announcement by its ID.
    /// </summary>
    /// <param name="id">The ID of the announcement to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves all announcements, optionally paginated.
    /// </summary>
    /// <param name="page">The page number to retrieve. If null, retrieves all announcements.</param>
    /// <param name="pageSize">The number of announcements per page. If null, retrieves all announcements.</param>
    /// <returns>A task representing the asynchronous operation, containing a collection of announcements.</returns>
    Task<IEnumerable<AnnouncementModel>> GetAllAsync(int? page = null, int? pageSize = null);

    /// <summary>
    /// Retrieves the details of an announcement by its ID, including additional information.
    /// </summary>
    /// <param name="id">The ID of the announcement to retrieve.</param>
    /// <param name="similarAnnouncementsCount">The number of similar announcements to include in the details.</param>
    /// <returns>The detailed information of the announcement with the specified ID.</returns>
    Task<AnnouncementDetailsModel?> GetByIdAsync(int id, int similarAnnouncementsCount = 3);
}