namespace Announcement.API.Profiles;

using Announcement.Models.Models;
using AutoMapper;
using Announcement = Announcement.DataAccess.Entities.Announcement;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<AnnouncementCreateModel, Announcement>();
        this.CreateMap<Announcement, AnnouncementModel>();
        this.CreateMap<Announcement, AnnouncementDetailsModel>();
    }
}