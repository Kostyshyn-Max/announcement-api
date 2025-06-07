namespace Announcement.BusinessLogic.Services;

using Announcement.BusinessLogic.Interfaces;
using Announcement.DataAccess.Interfaces;
using Announcement.DataAccess.EF;
using Announcement.DataAccess.Repositories;
using AutoMapper;
using Announcement.Models.Models;
using System.Text.RegularExpressions;
using Announcement = Announcement.DataAccess.Entities.Announcement;

/// <summary>
/// Provides implementation for announcement-related business logic operations.
/// </summary>
public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;

    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnnouncementService"/> class.
    /// </summary>
    /// <param name="repository">The repository used for accessing announcement data.</param>
    /// <param name="mapper">The mapper used for object-to-object mapping.</param>
    public AnnouncementService(IAnnouncementRepository repository, IMapper mapper)
    {
        this._announcementRepository = repository;
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
    /// <returns>
    /// The result, boolean value indicating whether the update was successful.
    /// </returns>
    public async Task<bool> UpdateAsync(int id, AnnouncementUpdateModel model)
    {
        var existingAnnouncement = await this._announcementRepository.GetByIdAsync(id);
        if (existingAnnouncement is null)
        {
            return false;
        }

        existingAnnouncement.Title = model.Title;
        existingAnnouncement.Description = model.Description;

        var result = await this._announcementRepository.UpdateAsync(existingAnnouncement);
        return result;
    }

    /// <summary>
    /// Deletes an announcement by its ID.
    /// </summary>
    /// <param name="id">The ID of the announcement to delete.</param>
    /// <returns>
    /// The result, boolean value indicating whether the deletion was successful.
    /// </returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var result = await this._announcementRepository.DeleteAsync(id);
        return result;
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
    /// <param name="similarAnnouncementsCount">The number of similar announcements to include in the result.</param>
    /// <returns>The detailed information of the announcement with the specified ID.</returns>
    public async Task<AnnouncementDetailsModel?> GetByIdAsync(int id, int similarAnnouncementsCount)
    {
        var announcementEntity = await this._announcementRepository.GetByIdAsync(id);
        if (announcementEntity is null)
        {
            return null;
        }

        var announcementModel = this._mapper.Map<AnnouncementDetailsModel>(announcementEntity);
        List<AnnouncementModel> similarAnnouncements = [];

        var words = ExtractWords(announcementModel.Title + " " + announcementModel.Description);

        var announcements = (await this._announcementRepository.GetAllAsync()).Where(a => !a.Equals(announcementEntity)).OrderBy(a => a.AddedDate);
        foreach (var announcement in announcements)
        {
            if (similarAnnouncements.Count >= similarAnnouncementsCount)
            {
                break;
            }

            string announcementText = announcement.Title + " " + announcement.Description;
            if (words.Any(word => Regex.IsMatch(announcementText, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase)))
            {
                similarAnnouncements.Add(this._mapper.Map<AnnouncementModel>(announcement));
            }
        }

        announcementModel.SimilarAnnouncements = similarAnnouncements;
        return announcementModel;
    }

    /// <summary>
    /// Extracts distinct words from the given text.
    /// </summary>
    /// <param name="text">The input text from which to extract words.</param>
    /// <returns>
    /// A list of unique words in lowercase extracted from the input text.
    /// Words are identified using a regular expression that matches word boundaries.
    /// </returns>
    private static List<string> ExtractWords(string text)
    {
        string pattern = @"\b[\w-]+\b";

        List<string> words = Regex.Matches(text, pattern).Select(x => x.Value.ToLower()).Distinct().ToList();
        return words;
    }
}