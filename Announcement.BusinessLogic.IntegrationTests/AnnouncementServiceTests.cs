namespace Announcement.BusinessLogic.Tests;

using Announcement.BusinessLogic.Interfaces;
using Announcement.BusinessLogic.Services;
using Announcement.Models.Models;
using Announcement.Models.Profiles;
using AutoMapper;
using FluentAssertions;

/// <summary>
/// Contains unit tests for the AnnouncementService class.
/// </summary>
public class AnnouncementServiceTests
{
    private readonly IAnnouncementService _announcementService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnnouncementServiceTests"/> class.
    /// Sets up the necessary dependencies for testing.
    /// </summary>
    public AnnouncementServiceTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        this._announcementService = new AnnouncementService(AnnouncementDbContextFactory.Instance, new Mapper(mapperConfiguration));
    }

    /// <summary>
    /// Tests that the DeleteAsync method throws a KeyNotFoundException
    /// when an invalid announcement ID is passed.
    /// </summary>
    [Test]
    public void Delete_WhenPassedInvalidId_ShouldThrownAnKeyNotFoundError()
    {
        int invalidAnnouncementId = -1;

        Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._announcementService.DeleteAsync(invalidAnnouncementId));
    }

    /// <summary>
    /// Tests that the GetByIdAsync method returns null
    /// when an invalid announcement ID is passed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task GetById_WhenPassedInvalidId_ShouldReturnNull()
    {
        int invalidAnnouncementId = -1;

        var announcement = await this._announcementService.GetByIdAsync(invalidAnnouncementId);
        Assert.Null(announcement);
    }

    /// <summary>
    /// Tests that the GetByIdAsync method returns the correct announcement
    /// when a valid announcement ID is passed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task GetById_WhenPassedValidId_ReturnAnnouncement()
    {
        int announcementId = 1;
        var data = DataSeed.GetDefaultAnnouncements().ToList()[0];
        AnnouncementDetailsModel expected = new()
        {
            Id = data.Id,
            Title = data.Title,
            Description = data.Description,
            AddedDate = data.AddedDate,
            SimilarAnnouncements = new List<AnnouncementModel>(),
        };

        var actual = await this._announcementService.GetByIdAsync(announcementId);

        Assert.NotNull(actual);
        actual.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Tests that the GetByIdAsync method returns similar announcements
    /// when a valid announcement ID is passed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task GetById_WhenPassedValidId_ShouldReturnSimilarAnnouncements()
    {
        int announcementId = 2;
        var data = DataSeed.GetDefaultAnnouncements().ToList();
        AnnouncementDetailsModel expected = new ()
        {
            Id = data[1].Id,
            Title = data[1].Title,
            Description = data[1].Description,
            AddedDate = data[1].AddedDate,
            SimilarAnnouncements = data.Skip(2)
                                        .OrderBy(a => a.AddedDate)
                                        .Take(3)
                                        .Select(a => new AnnouncementModel()
                                        {
                                            Id = a.Id,
                                            Title = a.Title,
                                            AddedDate = a.AddedDate,
                                        })
                                        .ToList(),
        };

        var actual = await this._announcementService.GetByIdAsync(announcementId);

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.SimilarAnnouncements.Count, Is.GreaterThan(0));
        for (int i = 1; i < actual.SimilarAnnouncements.Count; i++)
        {
            Assert.That(actual.SimilarAnnouncements[i].AddedDate, Is.GreaterThan(actual.SimilarAnnouncements[i - 1].AddedDate));
        }

        actual.Should().BeEquivalentTo(expected);
    }
}