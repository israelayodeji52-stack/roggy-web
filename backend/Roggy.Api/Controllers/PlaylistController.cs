using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Playlist.AddSongToPlaylist;
using Roggy.Application.Playlist.CreatePlaylist;
using Roggy.Application.Playlist.DeletePlaylist;
using Roggy.Application.Playlist.GetPlaylist;
using Roggy.Application.Playlist.GetPublicPlaylists;
using Roggy.Application.Playlist.GetUserPlaylists;
using Roggy.Application.Playlist.MakePlaylistPrivate;
using Roggy.Application.Playlist.MakePlaylistPublic;
using Roggy.Application.Playlist.RemoveSongFromPlaylist;
using Roggy.Application.Playlist.ReorderPlaylistSong;
using Roggy.Application.Playlist.UpdatePlaylist;
using Roggy.Contracts.Playlists;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistController : ControllerBase
{
    private readonly CreatePlaylistHandler _createPlaylistHandler;
    private readonly GetUserPlaylistsHandler _getUserPlaylistsHandler;
    private readonly GetPlaylistHandler _getPlaylistHandler;
    private readonly UpdatePlaylistHandler _updatePlaylistHandler;
    private readonly DeletePlaylistHandler _deletePlaylistHandler;
    private readonly AddSongToPlaylistHandler _addSongToPlaylistHandler;
    private readonly RemoveSongFromPlaylistHandler _removeSongFromPlaylistHandler;
    private readonly ReorderPlaylistSongHandler _reorderPlaylistSongHandler;
    private readonly MakePlaylistPublicHandler _makePlaylistPublicHandler;
    private readonly MakePlaylistPrivateHandler _makePlaylistPrivateHandler;
    private readonly GetPublicPlaylistsHandler _getPublicPlaylistsHandler;

    public PlaylistController(
        CreatePlaylistHandler createPlaylistHandler,
        GetUserPlaylistsHandler getUserPlaylistsHandler,
        GetPlaylistHandler getPlaylistHandler,
        UpdatePlaylistHandler updatePlaylistHandler,
        DeletePlaylistHandler deletePlaylistHandler,
        AddSongToPlaylistHandler addSongToPlaylistHandler,
        RemoveSongFromPlaylistHandler removeSongFromPlaylistHandler,
        ReorderPlaylistSongHandler reorderPlaylistSongHandler,
        MakePlaylistPublicHandler makePlaylistPublicHandler,
        MakePlaylistPrivateHandler makePlaylistPrivateHandler,
        GetPublicPlaylistsHandler getPublicPlaylistsHandler)
    {
        _createPlaylistHandler = createPlaylistHandler;
        _getUserPlaylistsHandler = getUserPlaylistsHandler;
        _getPlaylistHandler = getPlaylistHandler;
        _updatePlaylistHandler = updatePlaylistHandler;
        _deletePlaylistHandler = deletePlaylistHandler;
        _addSongToPlaylistHandler = addSongToPlaylistHandler;
        _removeSongFromPlaylistHandler = removeSongFromPlaylistHandler;
        _reorderPlaylistSongHandler = reorderPlaylistSongHandler;
        _makePlaylistPublicHandler = makePlaylistPublicHandler;
        _makePlaylistPrivateHandler = makePlaylistPrivateHandler;
        _getPublicPlaylistsHandler = getPublicPlaylistsHandler;
    }

    [HttpPost]
    public async Task<ActionResult<PlaylistResponse>> Create(
        [FromBody] CreatePlaylistRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var playlist = await _createPlaylistHandler.HandleAsync(
            new CreatePlaylistCommand(
                userId,
                request.Name),
            cancellationToken);

        return Ok(MapPlaylist(playlist));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlaylistResponse>>> GetMine(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var playlists = await _getUserPlaylistsHandler.HandleAsync(
            new GetUserPlaylistsQuery(userId),
            cancellationToken);

        var response = playlists
            .Select(MapPlaylist)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{playlistId:guid}")]
    public async Task<ActionResult<PlaylistResponse>> GetById(
        Guid playlistId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var playlist = await _getPlaylistHandler.HandleAsync(
            new GetPlaylistQuery(
                playlistId,
                userId),
            cancellationToken);

        if (playlist is null)
        {
            return NotFound(new
            {
                message = "Playlist was not found."
            });
        }

        return Ok(MapPlaylist(playlist));
    }

    [HttpGet("public")]
    public async Task<ActionResult<IReadOnlyList<PlaylistResponse>>> GetPublic(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var playlists = await _getPublicPlaylistsHandler.HandleAsync(
            new GetPublicPlaylistsQuery(limit),
            cancellationToken);

        var response = playlists
            .Select(MapPlaylist)
            .ToList();

        return Ok(response);
    }

    [HttpPut("{playlistId:guid}")]
    public async Task<ActionResult<PlaylistResponse>> Update(
        Guid playlistId,
        [FromBody] UpdatePlaylistRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var playlist = await _updatePlaylistHandler.HandleAsync(
                new UpdatePlaylistCommand(
                    playlistId,
                    userId,
                    request.Name,
                    request.Description,
                    request.CoverImageUrl),
                cancellationToken);

            return Ok(MapPlaylist(playlist));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }

    [HttpDelete("{playlistId:guid}")]
    public async Task<IActionResult> Delete(
        Guid playlistId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _deletePlaylistHandler.HandleAsync(
                new DeletePlaylistCommand(
                    playlistId,
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
    }

    [HttpPost("{playlistId:guid}/songs/{songId:guid}")]
    public async Task<ActionResult<PlaylistResponse>> AddSong(
        Guid playlistId,
        Guid songId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _addSongToPlaylistHandler.HandleAsync(
                new AddSongToPlaylistCommand(
                    playlistId,
                    userId,
                    songId),
                cancellationToken);

            var playlist = await _getPlaylistHandler.HandleAsync(
                new GetPlaylistQuery(
                    playlistId,
                    userId),
                cancellationToken);

            if (playlist is null)
            {
                return NotFound(new
                {
                    message = "Playlist was not found."
                });
            }

            return Ok(MapPlaylist(playlist));
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
    }

    [HttpDelete("{playlistId:guid}/songs/{songId:guid}")]
    public async Task<IActionResult> RemoveSong(
        Guid playlistId,
        Guid songId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _removeSongFromPlaylistHandler.HandleAsync(
                new RemoveSongFromPlaylistCommand(
                    playlistId,
                    userId,
                    songId),
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
    }

    [HttpPut("{playlistId:guid}/songs/{songId:guid}/position")]
    public async Task<IActionResult> ReorderSong(
        Guid playlistId,
        Guid songId,
        [FromBody] ReorderPlaylistSongRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            await _reorderPlaylistSongHandler.HandleAsync(
                new ReorderPlaylistSongCommand(
                    playlistId,
                    userId,
                    songId,
                    request.NewPosition),
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

    [HttpPost("{playlistId:guid}/public")]
    public async Task<ActionResult<PlaylistResponse>> MakePublic(
        Guid playlistId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var playlist = await _makePlaylistPublicHandler.HandleAsync(
                new MakePlaylistPublicCommand(
                    playlistId,
                    userId),
                cancellationToken);

            return Ok(MapPlaylist(playlist));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPost("{playlistId:guid}/private")]
    public async Task<ActionResult<PlaylistResponse>> MakePrivate(
        Guid playlistId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        try
        {
            var playlist = await _makePlaylistPrivateHandler.HandleAsync(
                new MakePlaylistPrivateCommand(
                    playlistId,
                    userId),
                cancellationToken);

            return Ok(MapPlaylist(playlist));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
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

    private static PlaylistResponse MapPlaylist(
        Roggy.Domain.Entities.Playlist playlist)
    {
        var songs = playlist.Items
            .OrderBy(item => item.Position)
            .Select(item =>
                new PlaylistSongResponse(
                    item.Song.Id,
                    item.Song.ArtistId,
                    item.Song.AlbumId,
                    item.Song.GenreId,
                    item.Song.Title,
                    item.Song.CoverImageUrl,
                    item.Song.Duration,
                    item.Song.ReleaseDate,
                    item.Song.TrackNumber,
                    item.Song.PlayCount))
            .ToList();

        return new PlaylistResponse(
            playlist.Id,
            playlist.UserId,
            playlist.Name,
            playlist.Description,
            playlist.CoverImageUrl,
            playlist.IsPublic,
            playlist.CreatedAt,
            playlist.UpdatedAt,
            songs);
    }
}