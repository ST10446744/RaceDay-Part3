using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.CSharp.Data;
using RaceDay.API.CSharp.Services;
using System.Security.Claims;

namespace RaceDay.API.CSharp.Controllers;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly RaceDayContext _context;
    private readonly AzureBlobStorageService _blobStorage;

    public ImagesController(
        RaceDayContext context,
        AzureBlobStorageService blobStorage)
    {
        _context = context;
        _blobStorage = blobStorage;
    }

    [HttpPost("events/{eventId:int}/banner")]
    [Authorize(Roles = "Organiser")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> UploadEventBanner(
        int eventId,
        IFormFile? file)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out int organiserId))
            return Unauthorized();

        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e => e.EventID == eventId);

        if (eventItem == null)
            return NotFound("Event not found.");

        if (eventItem.OrganiserID != organiserId)
            return Forbid();

        if (file == null || file.Length == 0)
            return BadRequest("Choose an image file.");

        try
        {
            var imagePath = await _blobStorage.UploadAsync(file, "events");

            eventItem.BannerImagePath = imagePath;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                eventItem.EventID,
                eventItem.BannerImagePath,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/api/images/file/{imagePath}"
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("profile")]
    [Authorize(Roles = "Participant")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> UploadProfilePicture(
        IFormFile? file)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out int participantId))
            return Unauthorized();

        var participant = await _context.Users
            .FirstOrDefaultAsync(u => u.UserID == participantId);

        if (participant == null)
            return NotFound("Participant profile not found.");

        if (file == null || file.Length == 0)
            return BadRequest("Choose an image file.");

        try
        {
            var imagePath = await _blobStorage.UploadAsync(file, "profiles");

            participant.ProfilePicturePath = imagePath;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                participant.UserID,
                participant.ProfilePicturePath,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/api/images/file/{imagePath}"
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("file/{**blobName}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImage(string? blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            return NotFound();

        if (!blobName.StartsWith("events/", StringComparison.OrdinalIgnoreCase) &&
            !blobName.StartsWith("profiles/", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound();
        }

        try
        {
            var image = await _blobStorage.DownloadAsync(blobName);
            return File(image.Stream, image.ContentType);
        }
        catch (RequestFailedException exception)
            when (exception.Status == 404)
        {
            return NotFound();
        }
    }
}