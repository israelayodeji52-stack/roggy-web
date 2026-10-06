using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Songs.CreateSong;
using Roggy.Application.Songs.DeleteSong;
using Roggy.Application.Songs.GetLikedSongs;
using Roggy.Application.Songs.GetRecentlyReleasedSongs;
using Roggy.Application.Songs.GetRecommendedSongs;
using Roggy.Application.Songs.GetSavedSongs;
using Roggy.Application.Songs.GetSavedSongStatus;
using Roggy.Application.Songs.GetSong;
using Roggy.Application.Songs.GetSongLikes;
using Roggy.Application.Songs.GetTrendingSongs;
using Roggy.Application.Songs.IncrementPlayCount;
using Roggy.Application.Songs.LikeSong;
using Roggy.Application.Songs.PublishSong;
using Roggy.Application.Songs.SaveSong;
using Roggy.Application.Songs.SavedSong;
using Roggy.Application.Songs.SearchSongs;
using Roggy.Application.Songs.UnlikeSong;
using Roggy.Application.Songs.UnpublishSong;
using Roggy.Application.Songs.UnsaveSong;
using Roggy.Application.Songs.UpdateSong;
using Roggy.Contracts.Songs;
using Roggy.Domain.Enums;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly CreateSongHandler _createSongHandler;
    private readonly GetSongHandler _getSongHandler;
    private readonly GetLikedSongsHandler _getLikedSongsHandler;
    private readonly GetSavedSongsHandler _getSavedSongsHandler;
    private readonly GetSavedSongStatusHandler _getSavedSongStatusHandler;
    private readonly UpdateSongHandler _updateSongHandler;
    private readonly DeleteSongHandler _deleteSongHandler;
    private readonly PublishSongHandler _publishSongHandler;
    private readonly UnpublishSongHandler _unpublishSongHandler;
    private readonly IncrementPlayCountHandler _incrementPlayCountHandler;
    private readonly LikeSongHandler _likeSongHandler;
    private readonly UnlikeSongHandler _unlikeSongHandler;
    private readonly GetSongLikesHandler _getSongLikesHandler;
    private readonly SavedSongHandler _savedSongHandler;
    private readonly UnsaveSongHandler _unsaveSongHandler;
    private readonly SearchSongsHandler _searchSongsHandler;
    private readonly GetRecentlyReleasedSongsHandler _getRecentlyReleasedSongsHandler;
    private readonly GetTrendingSongsHandler _getTrendingSongsHandler;
    private readonly GetRecommendedSongsHandler _getRecommendedSongsHandler;

    public SongsController(
        CreateSongHandler createSongHandler,
        GetSongHandler getSongHandler,
        GetLikedSongsHandler getLikedSongsHandler,
        GetSavedSongsHandler getSavedSongsHandler,
        GetSavedSongStatusHandler getSavedSongStatusHandler,
        UpdateSongHandler updateSongHandler,
        DeleteSongHandler deleteSongHandler,
        PublishSongHandler publishSongHandler,
        UnpublishSongHandler unpublishSongHandler,
        IncrementPlayCountHandler incrementPlayCountHandler,
        LikeSongHandler likeSongHandler,
        UnlikeSongHandler unlikeSongHandler,
        GetSongLikesHandler getSongLikesHandler,
        SavedSongHandler savedSongHandler,
        UnsaveSongHandler unsaveSongHandler,
        SearchSongsHandler searchSongsHandler,
        GetRecentlyReleasedSongsHandler getRecentlyReleasedSongsHandler,
        GetTrendingSongsHandler getTrendingSongsHandler,
        GetRecommendedSongsHandler getRecommendedSongsHandler)
    {
        _createSongHandler = createSongHandler;
        _getSongHandler = getSongHandler;
        _getLikedSongsHandler = getLikedSongsHandler;
        _getSavedSongsHandler = getSavedSongsHandler;
        _getSavedSongStatusHandler = getSavedSongStatusHandler;
        _updateSongHandler = updateSongHandler;
        _deleteSongHandler = deleteSongHandler;
        _publishSongHandler = publishSongHandler;
        _unpublishSongHandler = unpublishSongHandler;
        _incrementPlayCountHandler = incrementPlayCountHandler;
        _likeSongHandler = likeSongHandler;
        _unlikeSongHandler = unlikeSongHandler;
        _getSongLikesHandler = getSongLikesHandler;
        _savedSongHandler = savedSongHandler;
        _unsaveSongHandler = unsaveSongHandler;
        _searchSongsHandler = searchSongsHandler;
        _getRecentlyReleasedSongsHandler = getRecentlyReleasedSongsHandler;
        _getTrendingSongsHandler = getTrendingSongsHandler;
        _getRecommendedSongsHandler = getRecommendedSongsHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SongResponse>> Create(
        [FromBody] CreateSongRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(
                typeof(ContentRating),
                request.ContentRating))
        {
            return BadRequest("Invalid content rating.");
        }

        var command = new CreateSongCommand(
            request.ArtistId,
            request.AlbumId,
            request.GenreId,
            request.Title,
            request.Description,
            request.AudioUrl,
            request.CoverImageUrl,
            request.Duration,
            request.ReleaseDate,
            (ContentRating)request.ContentRating,
            request.AllowRemixing,
            request.TrackNumber);

        var song = await _createSongHandler.HandleAsync(
            command,
            cancellationToken);

        var response = new SongResponse(
            song.Id,
            song.ArtistId,
            song.AlbumId,
            song.GenreId,
            song.Title,
            song.Description,
            song.AudioUrl,
            song.CoverImageUrl,
            song.Duration,
            song.ReleaseDate,
            (int)song.ContentRating,
            song.AllowRemixing,
            song.IsPublished,
            song.PlayCount,
            song.TrackNumber,
            song.CreatedAt,
            song.UpdatedAt);

        return CreatedAtAction(
            nameof(GetById),
            new { id = song.Id },
            response);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SongResponse>>> Search(
        [FromQuery] string query,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new
            {
                message = "Search query is required."
            });
        }

        var songs = await _searchSongsHandler.HandleAsync(
            new SearchSongsQuery(query),
            cancellationToken);

        var response = songs
            .Select(song => new SongResponse(
                song.Id,
                song.ArtistId,
                song.AlbumId,
                song.GenreId,
                song.Title,
                song.Description,
                song.AudioUrl,
                song.CoverImageUrl,
                song.Duration,
                song.ReleaseDate,
                (int)song.ContentRating,
                song.AllowRemixing,
                song.IsPublished,
                song.PlayCount,
                song.TrackNumber,
                song.CreatedAt,
                song.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [HttpGet("recently-released")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<RecentlyReleasedSongResponse>>> GetRecentlyReleased(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var songs = await _getRecentlyReleasedSongsHandler.HandleAsync(
            new GetRecentlyReleasedSongsQuery(limit),
            cancellationToken);

        var response = songs
            .Select(song => new RecentlyReleasedSongResponse(
                song.Id,
                song.ArtistId,
                song.AlbumId,
                song.GenreId,
                song.Title,
                song.Description,
                song.AudioUrl,
                song.CoverImageUrl,
                song.Duration,
                song.ReleaseDate,
                (int)song.ContentRating,
                song.AllowRemixing,
                song.IsPublished,
                song.PlayCount,
                song.TrackNumber,
                song.CreatedAt,
                song.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [HttpGet("trending")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SongResponse>>> GetTrending(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var songs = await _getTrendingSongsHandler.HandleAsync(
            new GetTrendingSongsQuery(limit),
            cancellationToken);

        var response = songs
            .Select(song => new SongResponse(
                song.Id,
                song.ArtistId,
                song.AlbumId,
                song.GenreId,
                song.Title,
                song.Description,
                song.AudioUrl,
                song.CoverImageUrl,
                song.Duration,
                song.ReleaseDate,
                (int)song.ContentRating,
                song.AllowRemixing,
                song.IsPublished,
                song.PlayCount,
                song.TrackNumber,
                song.CreatedAt,
                song.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [HttpGet("recommended")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SongResponse>>> GetRecommended(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();

        var songs = await _getRecommendedSongsHandler.HandleAsync(
            new GetRecommendedSongsQuery(userId, limit),
            cancellationToken);

        var response = songs
            .Select(song => new SongResponse(
                song.Id,
                song.ArtistId,
                song.AlbumId,
                song.GenreId,
                song.Title,
                song.Description,
                song.AudioUrl,
                song.CoverImageUrl,
                song.Duration,
                song.ReleaseDate,
                (int)song.ContentRating,
                song.AllowRemixing,
                song.IsPublished,
                song.PlayCount,
                song.TrackNumber,
                song.CreatedAt,
                song.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<SongResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var song = await _getSongHandler.HandleAsync(
            new GetSongQuery(id),
            cancellationToken);

        if (song is null)
        {
            return NotFound("Song was not found.");
        }

        var response = new SongResponse(
            song.Id,
            song.ArtistId,
            song.AlbumId,
            song.GenreId,
            song.Title,
            song.Description,
            song.AudioUrl,
            song.CoverImageUrl,
            song.Duration,
            song.ReleaseDate,
            (int)song.ContentRating,
            song.AllowRemixing,
            song.IsPublished,
            song.PlayCount,
            song.TrackNumber,
            song.CreatedAt,
            song.UpdatedAt);

        return Ok(response);
    }

    [HttpGet("liked")]
    [Authorize]
    public async Task<ActionResult<List<SongResponse>>> GetLikedSongs(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var songs = await _getLikedSongsHandler.HandleAsync(
            new GetLikedSongsQuery(userId),
            cancellationToken);

        var response = songs
            .Select(song => new SongResponse(
                song.Id,
                song.ArtistId,
                song.AlbumId,
                song.GenreId,
                song.Title,
                song.Description,
                song.AudioUrl,
                song.CoverImageUrl,
                song.Duration,
                song.ReleaseDate,
                (int)song.ContentRating,
                song.AllowRemixing,
                song.IsPublished,
                song.PlayCount,
                song.TrackNumber,
                song.CreatedAt,
                song.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [HttpGet("saved")]
    [Authorize]
    public async Task<ActionResult<List<SongResponse>>> GetSavedSongs(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var songs = await _getSavedSongsHandler.HandleAsync(
            new GetSavedSongsQuery(userId),
            cancellationToken);

        var response = songs
            .Select(song => new SongResponse(
                song.Id,
                song.ArtistId,
                song.AlbumId,
                song.GenreId,
                song.Title,
                song.Description,
                song.AudioUrl,
                song.CoverImageUrl,
                song.Duration,
                song.ReleaseDate,
                (int)song.ContentRating,
                song.AllowRemixing,
                song.IsPublished,
                song.PlayCount,
                song.TrackNumber,
                song.CreatedAt,
                song.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SongResponse>> Update(
        Guid id,
        [FromBody] UpdateSongRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(
                typeof(ContentRating),
                request.ContentRating))
        {
            return BadRequest("Invalid content rating.");
        }

        try
        {
            var command = new UpdateSongCommand(
                id,
                request.AlbumId,
                request.GenreId,
                request.Title,
                request.Description,
                request.AudioUrl,
                request.CoverImageUrl,
                request.Duration,
                request.ReleaseDate,
                (ContentRating)request.ContentRating,
                request.AllowRemixing,
                request.TrackNumber);

            var song = await _updateSongHandler.HandleAsync(
                command,
                cancellationToken);

            if (song is null)
            {
                return NotFound("Song was not found.");
            }

            var response = new SongResponse(
                song.Id,
                song.ArtistId,
                song.AlbumId,
                song.GenreId,
                song.Title,
                song.Description,
                song.AudioUrl,
                song.CoverImageUrl,
                song.Duration,
                song.ReleaseDate,
                (int)song.ContentRating,
                song.AllowRemixing,
                song.IsPublished,
                song.PlayCount,
                song.TrackNumber,
                song.CreatedAt,
                song.UpdatedAt);

            return Ok(response);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _deleteSongHandler.HandleAsync(
            new DeleteSongCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SongActionResponse>> Publish(
        Guid id,
        CancellationToken cancellationToken)
    {
        var song = await _publishSongHandler.HandleAsync(
            new PublishSongCommand(id),
            cancellationToken);

        if (song is null)
        {
            return NotFound("Song was not found.");
        }

        var response = new SongActionResponse(
            song.Id,
            song.Title,
            song.IsPublished,
            song.UpdatedAt);

        return Ok(response);
    }

    [HttpPost("{id:guid}/unpublish")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SongActionResponse>> Unpublish(
        Guid id,
        CancellationToken cancellationToken)
    {
        var song = await _unpublishSongHandler.HandleAsync(
            new UnpublishSongCommand(id),
            cancellationToken);

        if (song is null)
        {
            return NotFound("Song was not found.");
        }

        var response = new SongActionResponse(
            song.Id,
            song.Title,
            song.IsPublished,
            song.UpdatedAt);

        return Ok(response);
    }

    [HttpPost("{id:guid}/play")]
    [Authorize]
    public async Task<ActionResult<SongPlayResponse>> Play(
        Guid id,
        CancellationToken cancellationToken)
    {
        var song = await _incrementPlayCountHandler.HandleAsync(
            new IncrementPlayCountCommand(id),
            cancellationToken);

        if (song is null)
        {
            return NotFound("Song was not found.");
        }

        var response = new SongPlayResponse(
            song.Id,
            song.Title,
            song.PlayCount);

        return Ok(response);
    }

    [HttpPost("{id:guid}/like")]
    [Authorize]
    public async Task<ActionResult<SongLikeResponse>> Like(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var songLike = await _likeSongHandler.HandleAsync(
            new LikeSongCommand(id, userId),
            cancellationToken);

        var response = new SongLikeResponse(
            songLike.Id,
            songLike.SongId,
            songLike.UserId,
            songLike.CreatedAt);

        return Ok(response);
    }

    [HttpDelete("{id:guid}/like")]
    [Authorize]
    public async Task<IActionResult> Unlike(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        await _unlikeSongHandler.HandleAsync(
            new UnlikeSongCommand(id, userId),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/likes")]
    [Authorize]
    public async Task<ActionResult<SongLikesResponse>> GetLikes(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var result = await _getSongLikesHandler.HandleAsync(
            new GetSongLikesQuery(id, userId),
            cancellationToken);

        var response = new SongLikesResponse(
            id,
            result.LikeCount,
            result.IsLiked);

        return Ok(response);
    }

    [HttpPost("{id:guid}/save")]
    [Authorize]
    public async Task<ActionResult<SavedSongResponse>> Save(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var savedSong = await _savedSongHandler.HandleAsync(
            new SaveSongCommand(id, userId),
            cancellationToken);

        var response = new SavedSongResponse(
            savedSong.Id,
            savedSong.SongId,
            savedSong.UserId,
            savedSong.CreatedAt);

        return Ok(response);
    }

    [HttpDelete("{id:guid}/save")]
    [Authorize]
    public async Task<IActionResult> Unsave(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        await _unsaveSongHandler.HandleAsync(
            new UnsaveSongCommand(id, userId),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/save")]
    [Authorize]
    public async Task<ActionResult<SavedSongStatusResponse>> GetSavedSongStatus(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var isSaved = await _getSavedSongStatusHandler.HandleAsync(
            new GetSavedSongStatusQuery(id, userId),
            cancellationToken);

        var response = new SavedSongStatusResponse(
            id,
            isSaved);

        return Ok(response);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException(
                "User ID could not be determined.");
        }

        return userId;
    }
}