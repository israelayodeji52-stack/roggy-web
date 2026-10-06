using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.ListeningHistory.DeleteListeningHistoryEntry;
using Roggy.Application.ListeningHistory.GetListeningHistory;
using Roggy.Application.ListeningHistory.GetListeningHistoryEntry;
using Roggy.Application.ListeningHistory.RecordListeningHistory;
using Roggy.Contracts.ListeningHistory;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ListeningHistoryController : ControllerBase
{
    private readonly RecordListeningHistoryHandler
        _recordListeningHistoryHandler;

    private readonly GetListeningHistoryHandler
        _getListeningHistoryHandler;

    private readonly GetListeningHistoryEntryHandler
        _getListeningHistoryEntryHandler;

    private readonly DeleteListeningHistoryEntryHandler
        _deleteListeningHistoryEntryHandler;

    public ListeningHistoryController(
        RecordListeningHistoryHandler recordListeningHistoryHandler,
        GetListeningHistoryHandler getListeningHistoryHandler,
        GetListeningHistoryEntryHandler getListeningHistoryEntryHandler,
        DeleteListeningHistoryEntryHandler deleteListeningHistoryEntryHandler)
    {
        _recordListeningHistoryHandler =
            recordListeningHistoryHandler;

        _getListeningHistoryHandler =
            getListeningHistoryHandler;

        _getListeningHistoryEntryHandler =
            getListeningHistoryEntryHandler;

        _deleteListeningHistoryEntryHandler =
            deleteListeningHistoryEntryHandler;
    }

    [HttpPost]
    public async Task<ActionResult<ListeningHistoryResponse>> Record(
        [FromBody] RecordListeningHistoryRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var history =
                await _recordListeningHistoryHandler.HandleAsync(
                    new RecordListeningHistoryCommand(
                        userId,
                        request.SongId,
                        request.StartedAt,
                        request.CompletedAt,
                        request.DurationPlayed),
                    cancellationToken);

            return Ok(MapHistory(history));
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
        IReadOnlyList<ListeningHistoryResponse>>> GetMine(
        [FromQuery] int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();

        var history =
            await _getListeningHistoryHandler.HandleAsync(
                new GetListeningHistoryQuery(
                    userId,
                    limit),
                cancellationToken);

        var response = history
            .Select(MapHistory)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{historyId:guid}")]
    public async Task<ActionResult<ListeningHistoryResponse>> GetById(
        Guid historyId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var history =
                await _getListeningHistoryEntryHandler.HandleAsync(
                    new GetListeningHistoryEntryQuery(
                        historyId,
                        userId),
                    cancellationToken);

            if (history is null)
            {
                return NotFound(new
                {
                    message =
                        "Listening history entry was not found."
                });
            }

            return Ok(MapHistory(history));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpDelete("{historyId:guid}")]
    public async Task<IActionResult> Delete(
        Guid historyId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _deleteListeningHistoryEntryHandler.HandleAsync(
                new DeleteListeningHistoryEntryCommand(
                    historyId,
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

    private static ListeningHistoryResponse MapHistory(
        Roggy.Domain.Entities.ListeningHistory history)
    {
        return new ListeningHistoryResponse(
            history.Id,
            history.UserId,
            history.SongId,
            history.Song.Title,
            history.Song.CoverImageUrl,
            history.StartedAt,
            history.CompletedAt,
            history.DurationPlayed,
            history.CreatedAt,
            history.UpdatedAt);
    }
}