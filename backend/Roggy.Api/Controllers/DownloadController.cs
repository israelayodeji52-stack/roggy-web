using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Downloads.DeleteDownloadHistoryEntry;
using Roggy.Application.Downloads.DownloadSong;
using Roggy.Application.Downloads.GetDownloadHistory;
using Roggy.Application.Downloads.GetDownloadHistoryEntry;
using Roggy.Contracts.Downloads;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DownloadController : ControllerBase
{
    private readonly DownloadSongHandler _downloadSongHandler;

    private readonly GetDownloadHistoryHandler
        _getDownloadHistoryHandler;

    private readonly GetDownloadHistoryEntryHandler
        _getDownloadHistoryEntryHandler;

    private readonly DeleteDownloadHistoryEntryHandler
        _deleteDownloadHistoryEntryHandler;

    public DownloadController(
        DownloadSongHandler downloadSongHandler,
        GetDownloadHistoryHandler getDownloadHistoryHandler,
        GetDownloadHistoryEntryHandler getDownloadHistoryEntryHandler,
        DeleteDownloadHistoryEntryHandler deleteDownloadHistoryEntryHandler)
    {
        _downloadSongHandler = downloadSongHandler;
        _getDownloadHistoryHandler = getDownloadHistoryHandler;
        _getDownloadHistoryEntryHandler =
            getDownloadHistoryEntryHandler;
        _deleteDownloadHistoryEntryHandler =
            deleteDownloadHistoryEntryHandler;
    }

    [HttpPost("{songId:guid}")]
    public async Task<ActionResult<DownloadHistoryResponse>> Download(
        Guid songId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var downloadHistory =
                await _downloadSongHandler.HandleAsync(
                    new DownloadSongCommand(
                        userId,
                        songId),
                    cancellationToken);

            return Ok(MapDownloadHistory(downloadHistory));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<
        IReadOnlyList<DownloadHistoryResponse>>> GetMine(
        [FromQuery] int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();

        try
        {
            var history =
                await _getDownloadHistoryHandler.HandleAsync(
                    new GetDownloadHistoryQuery(
                        userId,
                        limit),
                    cancellationToken);

            var response = history
                .Select(MapDownloadHistory)
                .ToList();

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpGet("{downloadHistoryId:guid}")]
    public async Task<ActionResult<DownloadHistoryResponse>> GetById(
        Guid downloadHistoryId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var downloadHistory =
                await _getDownloadHistoryEntryHandler.HandleAsync(
                    new GetDownloadHistoryEntryQuery(
                        downloadHistoryId,
                        userId),
                    cancellationToken);

            if (downloadHistory is null)
            {
                return NotFound(new
                {
                    message =
                        "Download history entry was not found."
                });
            }

            return Ok(MapDownloadHistory(downloadHistory));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpDelete("{downloadHistoryId:guid}")]
    public async Task<IActionResult> Delete(
        Guid downloadHistoryId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _deleteDownloadHistoryEntryHandler.HandleAsync(
                new DeleteDownloadHistoryEntryCommand(
                    downloadHistoryId,
                    userId),
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedAccessException(
                "Authenticated user ID is missing.");
        }

        return userId;
    }

    private static DownloadHistoryResponse MapDownloadHistory(
        Roggy.Domain.Entities.DownloadHistory history)
    {
        return new DownloadHistoryResponse(
            history.Id,
            history.UserId,
            history.SongId,
            history.Song.Title,
            history.Song.CoverImageUrl,
            history.DownloadedAt,
            history.CreatedAt,
            history.UpdatedAt);
    }
}