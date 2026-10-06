using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Remixes.CreateRemix;
using Roggy.Application.Remixes.DeleteRemix;
using Roggy.Application.Remixes.GetRemix;
using Roggy.Application.Remixes.GetSongRemixes;
using Roggy.Application.Remixes.PublishRemix;
using Roggy.Application.Remixes.UnpublishRemix;
using Roggy.Application.Remixes.UpdateRemix;
using Roggy.Contracts.Remixes;
using Roggy.Domain.Enums;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RemixController : ControllerBase
{
    private readonly CreateRemixHandler _createRemixHandler;
    private readonly GetRemixHandler _getRemixHandler;
    private readonly GetSongRemixesHandler _getSongRemixesHandler;
    private readonly UpdateRemixHandler _updateRemixHandler;
    private readonly DeleteRemixHandler _deleteRemixHandler;
    private readonly PublishRemixHandler _publishRemixHandler;
    private readonly UnpublishRemixHandler _unpublishRemixHandler;

    public RemixController(
        CreateRemixHandler createRemixHandler,
        GetRemixHandler getRemixHandler,
        GetSongRemixesHandler getSongRemixesHandler,
        UpdateRemixHandler updateRemixHandler,
        DeleteRemixHandler deleteRemixHandler,
        PublishRemixHandler publishRemixHandler,
        UnpublishRemixHandler unpublishRemixHandler)
    {
        _createRemixHandler = createRemixHandler;
        _getRemixHandler = getRemixHandler;
        _getSongRemixesHandler = getSongRemixesHandler;
        _updateRemixHandler = updateRemixHandler;
        _deleteRemixHandler = deleteRemixHandler;
        _publishRemixHandler = publishRemixHandler;
        _unpublishRemixHandler = unpublishRemixHandler;
    }

    [HttpPost]
    public async Task<ActionResult<RemixResponse>> Create(
        [FromBody] CreateRemixRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var remix = await _createRemixHandler.HandleAsync(
                new CreateRemixCommand(
                    userId,
                    request.OriginalSongId,
                    request.Title,
                    request.AudioUrl,
                    request.CoverImageUrl,
                    request.Duration),
                cancellationToken);

            return Ok(MapRemix(remix));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
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

    [HttpGet("{remixId:guid}")]
    public async Task<ActionResult<RemixResponse>> GetById(
        Guid remixId,
        CancellationToken cancellationToken)
    {
        try
        {
            var remix = await _getRemixHandler.HandleAsync(
                new GetRemixQuery(remixId),
                cancellationToken);

            if (remix is null)
            {
                return NotFound(new
                {
                    message = "Remix was not found."
                });
            }

            return Ok(MapRemix(remix));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpGet("song/{songId:guid}")]
    public async Task<ActionResult<
        IReadOnlyList<RemixResponse>>> GetSongRemixes(
        Guid songId,
        [FromQuery] int limit = 50,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var remixes = await _getSongRemixesHandler.HandleAsync(
                new GetSongRemixesQuery(
                    songId,
                    limit),
                cancellationToken);

            var response = remixes
                .Select(MapRemix)
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

    [HttpPut("{remixId:guid}")]
    public async Task<ActionResult<RemixResponse>> Update(
        Guid remixId,
        [FromBody] UpdateRemixRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (!Enum.IsDefined(
                typeof(ContentRating),
                request.ContentRating))
        {
            return BadRequest(new
            {
                message = "Invalid content rating."
            });
        }

        try
        {
            var remix = await _updateRemixHandler.HandleAsync(
                new UpdateRemixCommand(
                    remixId,
                    userId,
                    request.Title,
                    request.AudioUrl,
                    request.CoverImageUrl,
                    request.Duration,
                    (ContentRating)request.ContentRating),
                cancellationToken);

            return Ok(MapRemix(remix));
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

    [HttpDelete("{remixId:guid}")]
    public async Task<IActionResult> Delete(
        Guid remixId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _deleteRemixHandler.HandleAsync(
                new DeleteRemixCommand(
                    remixId,
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

    [HttpPost("{remixId:guid}/publish")]
    public async Task<ActionResult<RemixResponse>> Publish(
        Guid remixId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var remix = await _publishRemixHandler.HandleAsync(
                new PublishRemixCommand(
                    remixId,
                    userId),
                cancellationToken);

            return Ok(MapRemix(remix));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new
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

    [HttpPost("{remixId:guid}/unpublish")]
    public async Task<ActionResult<RemixResponse>> Unpublish(
        Guid remixId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var remix = await _unpublishRemixHandler.HandleAsync(
                new UnpublishRemixCommand(
                    remixId,
                    userId),
                cancellationToken);

            return Ok(MapRemix(remix));
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

    private static RemixResponse MapRemix(
        Roggy.Domain.Entities.Remix remix)
    {
        return new RemixResponse(
            remix.Id,
            remix.OriginalSongId,
            remix.RemixerArtistId,
            remix.Title,
            remix.AudioUrl,
            remix.CoverImageUrl,
            remix.Duration,
            remix.ContentRating.ToString(),
            remix.IsPublished,
            remix.CreatedAt,
            remix.UpdatedAt);
    }
}