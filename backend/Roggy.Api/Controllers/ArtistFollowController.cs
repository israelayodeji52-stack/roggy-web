using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Artists.FollowArtist;
using Roggy.Application.Artists.GetArtistFollowStatus;
using Roggy.Application.Artists.GetArtistFollowerCount;
using Roggy.Application.Artists.GetFollowedArtists;
using Roggy.Application.Artists.UnfollowArtist;
using Roggy.Domain.Entities;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArtistFollowController : ControllerBase
{
    private readonly FollowArtistHandler _followArtistHandler;
    private readonly UnfollowArtistHandler _unfollowArtistHandler;
    private readonly GetArtistFollowStatusHandler _getArtistFollowStatusHandler;
    private readonly GetFollowedArtistsHandler _getFollowedArtistsHandler;
    private readonly GetArtistFollowerCountHandler _getArtistFollowerCountHandler;

    public ArtistFollowController(
        FollowArtistHandler followArtistHandler,
        UnfollowArtistHandler unfollowArtistHandler,
        GetArtistFollowStatusHandler getArtistFollowStatusHandler,
        GetFollowedArtistsHandler getFollowedArtistsHandler,
        GetArtistFollowerCountHandler getArtistFollowerCountHandler)
    {
        _followArtistHandler = followArtistHandler;
        _unfollowArtistHandler = unfollowArtistHandler;
        _getArtistFollowStatusHandler = getArtistFollowStatusHandler;
        _getFollowedArtistsHandler = getFollowedArtistsHandler;
        _getArtistFollowerCountHandler = getArtistFollowerCountHandler;
    }

    [HttpPost("{artistId:guid}")]
    public async Task<IActionResult> Follow(
        Guid artistId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _followArtistHandler.HandleAsync(
                new FollowArtistCommand(userId, artistId),
                cancellationToken);

            return Ok(new
            {
                message = "Artist followed successfully."
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpDelete("{artistId:guid}")]
    public async Task<IActionResult> Unfollow(
        Guid artistId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _unfollowArtistHandler.HandleAsync(
                new UnfollowArtistCommand(userId, artistId),
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpGet("{artistId:guid}/status")]
    public async Task<ActionResult<bool>> GetStatus(
        Guid artistId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var isFollowing =
                await _getArtistFollowStatusHandler.HandleAsync(
                    new GetArtistFollowStatusQuery(
                        userId,
                        artistId),
                    cancellationToken);

            return Ok(new
            {
                isFollowing
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpGet("following")]
    public async Task<ActionResult<IReadOnlyList<ArtistProfile>>> GetFollowedArtists(
        [FromQuery] int limit = 50,
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();

        try
        {
            var artists =
                await _getFollowedArtistsHandler.HandleAsync(
                    new GetFollowedArtistsQuery(
                        userId,
                        limit),
                    cancellationToken);

            return Ok(artists);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpGet("{artistId:guid}/followers/count")]
    public async Task<ActionResult<int>> GetFollowerCount(
        Guid artistId,
        CancellationToken cancellationToken)
    {
        try
        {
            var count =
                await _getArtistFollowerCountHandler.HandleAsync(
                    new GetArtistFollowerCountQuery(artistId),
                    cancellationToken);

            return Ok(new
            {
                followerCount = count
            });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedAccessException(
                "Authenticated user ID is missing.");
        }

        return userId;
    }
}