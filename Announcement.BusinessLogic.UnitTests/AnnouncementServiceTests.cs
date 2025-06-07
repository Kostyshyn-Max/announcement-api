using Announcement.Models.Models;

namespace Announcement.BusinessLogic.UnitTests;

using Announcement.BusinessLogic.Interfaces;
using Announcement.BusinessLogic.Services;
using Announcement.DataAccess.Interfaces;
using Announcement.Models.Profiles;
using AutoMapper;
using Moq;
using Announcement = DataAccess.Entities.Announcement;

public class AnnouncementServiceTests
{
    private IAnnouncementService _announcementService;

    private Mock<IAnnouncementRepository> _mockRepository;
    
    [SetUp]
    public void Setup()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        
        _mockRepository = new Mock<IAnnouncementRepository>();
        _announcementService = new AnnouncementService(_mockRepository.Object, new Mapper(mapperConfiguration));
    }

    [Test]
    public void GetById_WhenAnnouncementNotFound_ShouldReturnNull()
    {
        // Arrange
        int invalidId = 999;
        _mockRepository.Setup(repo => repo.GetByIdAsync(invalidId)).ReturnsAsync((Announcement?)null);

        // Act
        var result = _announcementService.GetByIdAsync(invalidId, 3).Result;

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void GetById_WhenNoSimilarAnnouncementsFound_ShouldReturnAnnouncementWithEmptySimilarList()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "System Maintenance Scheduled",
                Description = "Maintenance window on Sunday.",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "Random Unrelated Announcement",
                Description = "This is a completely unrelated post.",
                AddedDate = new DateTime(2025, 06, 03)
            }
        };

        var mainAnnouncement = announcements.First(a => a.Id == announcementId);

        _mockRepository.Setup(repo => repo.GetByIdAsync(announcementId)).ReturnsAsync(mainAnnouncement);
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 3).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(mainAnnouncement.Id));
        Assert.That(result.SimilarAnnouncements, Is.Empty);
    }

    [Test]
    public void GetById_WhenMoreSimilarAnnouncementsExist_ShouldLimitByCount()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "System Maintenance Scheduled",
                Description = "Platform maintenance on Sunday.",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "Maintenance Tips",
                Description = "Tips during scheduled maintenance.",
                AddedDate = new DateTime(2025, 06, 02)
            },
            new Announcement
            {
                Id = 3,
                Title = "System Downtime",
                Description = "Expect downtime due to maintenance.",
                AddedDate = new DateTime(2025, 06, 03)
            },
            new Announcement
            {
                Id = 4,
                Title = "Unrelated Announcement",
                Description = "Not similar at all.",
                AddedDate = new DateTime(2025, 06, 04)
            }
        };

        var mainAnnouncement = announcements.First(a => a.Id == announcementId);

        _mockRepository.Setup(repo => repo.GetByIdAsync(announcementId)).ReturnsAsync(mainAnnouncement);
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 1).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(mainAnnouncement.Id));
        Assert.That(result.SimilarAnnouncements.Count, Is.EqualTo(1));
    }

    [Test]
    public void GetById_ShouldMatchWordsWithDifferentCasing()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "System Update",
                Description = "The system will be updated tonight.",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "SYSTEM outage",
                Description = "Unexpected issue occurred.",
                AddedDate = new DateTime(2025, 06, 02)
            }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(announcementId)).ReturnsAsync(announcements[0]);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 3).Result;

        // Assert
        Assert.That(result.SimilarAnnouncements.Count, Is.EqualTo(1));
        Assert.That(result.SimilarAnnouncements.First().Id, Is.EqualTo(2));
    }
    
    [Test]
    public void GetById_ShouldMatchWordsNextToPunctuation()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "Update: New Features",
                Description = "Check out what's new!",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "Exciting new features",
                Description = "More updates coming soon.",
                AddedDate = new DateTime(2025, 06, 02)
            }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(announcementId)).ReturnsAsync(announcements[0]);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 3).Result;

        // Assert
        Assert.That(result.SimilarAnnouncements.Count, Is.EqualTo(1));
        Assert.That(result.SimilarAnnouncements.First().Id, Is.EqualTo(2));
    }
    
    [Test]
    public void GetById_ShouldMatchHyphenatedWords()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "System-wide Update",
                Description = "Full platform update.",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "Upcoming system-wide downtime",
                Description = "Prepare accordingly.",
                AddedDate = new DateTime(2025, 06, 02)
            },
            new Announcement
            {
                Id = 3,
                Title = "Upcoming system",
                Description = "Prepare accordingly.",
                AddedDate = new DateTime(2025, 06, 05)
            }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(announcementId)).ReturnsAsync(announcements[0]);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 3).Result;

        // Assert
        Assert.That(result.SimilarAnnouncements.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void GetById_ShouldMatchWordsWithDigits()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "Update v2.0 Released",
                Description = "New version 2.0 is now live.",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "v2.0 Patch Notes",
                Description = "Fixes and improvements included.",
                AddedDate = new DateTime(2025, 06, 02)
            }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(announcementId)).ReturnsAsync(announcements[0]);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 3).Result;

        // Assert
        Assert.That(result.SimilarAnnouncements.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void GetById_ShouldMatchPureDigits()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "Meeting at 10",
                Description = "The session starts at 10 sharp.",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "Reminder: 10 AM Start",
                Description = "Don't be late for the 10 AM meeting.",
                AddedDate = new DateTime(2025, 06, 02)
            }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(announcementId)).ReturnsAsync(announcements[0]);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 3).Result;

        // Assert
        Assert.That(result.SimilarAnnouncements.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void GetById_ShouldReturnSimilarAnnouncementsSortedByDate()
    {
        // Arrange
        int announcementId = 1;
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = 1,
                Title = "System Maintenance",
                Description = "Scheduled maintenance this week.",
                AddedDate = new DateTime(2025, 06, 01)
            },
            new Announcement
            {
                Id = 2,
                Title = "Maintenance Info",
                Description = "More details about the system maintenance.",
                AddedDate = new DateTime(2025, 06, 03)
            },
            new Announcement
            {
                Id = 3,
                Title = "Maintenance Advisory",
                Description = "Please read the maintenance guidelines.",
                AddedDate = new DateTime(2025, 06, 02)
            },
            new Announcement
            {
                Id = 4,
                Title = "System Maintenance",
                Description = "Scheduled maintenance this week.",
                AddedDate = new DateTime(2025, 06, 01)
            },
        };

        var expectedOrder = new List<int> { 4, 3 }; // Sorted by AddedDate

        _mockRepository.Setup(r => r.GetByIdAsync(announcementId)).ReturnsAsync(announcements[0]);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(announcements);

        // Act
        var result = _announcementService.GetByIdAsync(announcementId, 2).Result;

        // Assert
        Assert.That(result.SimilarAnnouncements.Count, Is.EqualTo(2));
        var actualOrder = result.SimilarAnnouncements.Select(a => a.Id).ToList();
        CollectionAssert.AreEqual(expectedOrder, actualOrder);
    }
}