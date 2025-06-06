namespace Announcement.DataAccess.Interfaces;

/// <summary>
/// Interface for the announcement repository, inheriting from the generic CRUD repository interface.
/// </summary>
public interface IAnnouncementRepository : ICrudRepository<Entities.Announcement>
{
}