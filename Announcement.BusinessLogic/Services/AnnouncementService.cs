using System.Collections;

namespace Announcement.BusinessLogic.Services;

using Announcement.BusinessLogic.Interfaces;
using Announcement.DataAccess.Interfaces;
using Announcement.DataAccess.EF;
using Announcement.DataAccess.Repositories;
using AutoMapper;
using Announcement.Models.Models;
using Announcement = Announcement.DataAccess.Entities.Announcement;

/// <summary>
/// Provides implementation for announcement-related business logic operations.
/// </summary>
public class AnnouncementService : IAnnouncementService
{
    /// <summary>
    /// Repository for managing announcement data.
    /// </summary>
    private readonly IAnnouncementRepository _announcementRepository;

    /// <summary>
    /// Mapper for transforming objects between different layers.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnnouncementService"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    /// <param name="mapper">The mapper used for object transformations.</param>
    public AnnouncementService(ApplicationDbContext context, IMapper mapper)
    {
        this._announcementRepository = new AnnouncementRepository(context);
        this._mapper = mapper;
    }

    /// <summary>
    /// Creates a new announcement.
    /// </summary>
    /// <param name="model">The model containing the details of the announcement to create.</param>
    /// <returns>The ID of the created announcement.</returns>
    public async Task<int> CreateAsync(AnnouncementCreateModel model)
    {
        var announcementEntity = this._mapper.Map<AnnouncementCreateModel, Announcement>(model);
        announcementEntity.AddedDate = DateTime.UtcNow;

        int announcementId = await this._announcementRepository.CreateAsync(announcementEntity);

        return announcementId;
    }

    /// <summary>
    /// Updates an existing announcement with the specified ID using the provided update model.
    /// </summary>
    /// <param name="id">The ID of the announcement to update.</param>
    /// <param name="model">The model containing the updated details of the announcement.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the announcement with the specified ID is not found.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateAsync(int id, AnnouncementUpdateModel model)
    {
        var existingAnnouncement = await this._announcementRepository.GetByIdAsync(id);
        if (existingAnnouncement is null)
        {
            throw new KeyNotFoundException($"Announcement with ID: {id} was not found.");
        }

        existingAnnouncement.Title = model.Title;
        existingAnnouncement.Description = model.Description;

        await this._announcementRepository.UpdateAsync(existingAnnouncement);
    }

    /// <summary>
    /// Deletes an announcement by its ID.
    /// </summary>
    /// <param name="id">The ID of the announcement to delete.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an error occurs during the deletion process.
    /// </exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(int id)
    {
        try
        {
            await this._announcementRepository.DeleteAsync(id);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Retrieves all announcements, optionally paginated.
    /// </summary>
    /// <param name="page">The page number to retrieve. If null, retrieves all announcements.</param>
    /// <param name="pageSize">The number of announcements per page. If null, retrieves all announcements.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a collection of announcement models.
    /// </returns>
    public async Task<IEnumerable<AnnouncementModel>> GetAllAsync(int? page = null, int? pageSize = null)
    {
        IEnumerable<Announcement> announcements;

        if (page is not null && pageSize is not null)
        {
            announcements = await this._announcementRepository.GetAllAsync((int)page, (int)pageSize);
        }
        else
        {
            announcements = await this._announcementRepository.GetAllAsync();
        }

        var announcementModels = announcements.Select(a => this._mapper.Map<AnnouncementModel>(a)).ToList();
        return announcementModels;
    }

    /// <summary>
    /// Retrieves an announcement by its ID, including additional information and a specified number of similar announcements.
    /// </summary>
    /// <param name="id">The ID of the announcement to retrieve.</param>
    /// <param name="similarAnnouncementsCount">The number of similar announcements to include in the result. Defaults to 3.</param>
    /// <returns>The detailed information of the announcement with the specified ID.</returns>
    public async Task<AnnouncementDetailsModel?> GetByIdAsync(int id, int similarAnnouncementsCount = 3)
    {
        var announcementEntity = await this._announcementRepository.GetByIdAsync(id);
        var announcementModel = this._mapper.Map<AnnouncementDetailsModel>(announcementEntity);
        List<AnnouncementModel> similarAnnouncements = [];

        var announcements = (await this._announcementRepository.GetAllAsync()).Where(a => !a.Equals(announcementEntity)).OrderBy(a => a.AddedDate);
        foreach (var announcement in announcements)
        {
            if (similarAnnouncements.Count > 3)
            {
                break;
            }

            var titleWords = announcement.Title.Split(" ");
            var descriptionWords = announcement.Description.Split(" ");

            if (titleWords.Any(word => announcement.Title.Contains(word)) &&
                descriptionWords.Any(word => announcement.Description.Contains(word)))
            {
                similarAnnouncements.Add(this._mapper.Map<AnnouncementModel>(announcement));
            }
        }

        announcementModel.SimilarAnnouncements = similarAnnouncements;
        return announcementModel;
    }
}