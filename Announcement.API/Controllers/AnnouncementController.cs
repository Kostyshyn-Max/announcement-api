namespace Announcement.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Announcement.BusinessLogic.Interfaces;
using Announcement.Models.Models;

/// <summary>
/// Represents a controller for managing announcements.
/// </summary>
[ApiController]
[Route("/api/[controller]/")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnnouncementController"/> class.
    /// </summary>
    /// <param name="announcementService">The service to handle announcement operations.</param>
    public AnnouncementController(IAnnouncementService announcementService)
    {
        this._announcementService = announcementService;
    }

    /// <summary>
    /// Creates a new announcement.
    /// </summary>
    /// <param name="model">The model containing the details of the announcement to create.</param>
    /// <returns>The ID of the created announcement.</returns>
    [HttpPost]
    [Route("create")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create([FromBody] AnnouncementCreateModel model)
    {
        var announcementId = await this._announcementService.CreateAsync(model);
        return this.Ok(announcementId);
    }

    /// <summary>
    /// Retrieves all announcements, optionally paginated.
    /// </summary>
    /// <param name="page">The page number to retrieve. If null, retrieves all announcements.</param>
    /// <param name="pageSize">The number of announcements per page. If null, retrieves all announcements.</param>
    /// <returns>A list of announcements.</returns>
    [HttpGet]
    [ProducesResponseType<List<AnnouncementModel>>(StatusCodes.Status200OK)]
    public async Task<List<AnnouncementModel>> GetAll([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        var announcements = await this._announcementService.GetAllAsync(page, pageSize);
        return announcements.ToList();
    }

    /// <summary>
    /// Retrieves an announcement by its ID.
    /// </summary>
    /// <param name="id">The ID of the announcement to retrieve.</param>
    /// <param name="similarAnnouncementsCount">The number of similar announcements to include in the result. Defaults to 3.</param>
    /// <returns>The announcement with the specified ID.</returns>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType<AnnouncementModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnnouncementModel>> GetById([FromRoute] int id, [FromQuery] int similarAnnouncementsCount = 3)
    {
        var announcement = await this._announcementService.GetByIdAsync(id, similarAnnouncementsCount);

        if (announcement is null)
        {
            return this.NotFound();
        }

        return this.Ok(announcement);
    }

    /// <summary>
    /// Deletes an announcement by its ID.
    /// </summary>
    /// <param name="id">The ID of the announcement to delete.</param>
    /// <returns>A response indicating the result of the deletion.</returns>
    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var result = await this._announcementService.DeleteAsync(id);
        if (result)
        {
            return this.NoContent();
        }

        return this.BadRequest();
    }

    /// <summary>
    /// Updates an existing announcement by its ID.
    /// </summary>
    /// <param name="id">The ID of the announcement to update.</param>
    /// <param name="model">The model containing the updated details of the announcement.</param>
    /// <returns>A response indicating the result of the update operation.</returns>
    [HttpPut]
    [Route("update/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] AnnouncementUpdateModel model)
    {
        var result = await this._announcementService.UpdateAsync(id, model);
        if (result)
        {
            return this.NoContent();
        }

        return this.BadRequest();
    }
}