using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Albums.CreateAlbum;
using Roggy.Application.Albums.DeleteAlbum;
using Roggy.Application.Albums.GetAlbum;
using Roggy.Application.Albums.GetArtistAlbums;
using Roggy.Application.Albums.PublishAlbum;
using Roggy.Application.Albums.UnpublishAlbum;
using Roggy.Application.Albums.UpdateAlbum;
using Roggy.Contracts.Albums;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly CreateAlbumHandler _createAlbumHandler;
    private readonly GetAlbumHandler _getAlbumHandler;
    private readonly GetArtistAlbumsHandler _getArtistAlbumsHandler;
    private readonly UpdateAlbumHandler _updateAlbumHandler;
    private readonly DeleteAlbumHandler _deleteAlbumHandler;
    private readonly PublishAlbumHandler _publishAlbumHandler;
    private readonly UnpublishAlbumHandler _unpublishAlbumHandler;

    public AlbumsController(
        CreateAlbumHandler createAlbumHandler,
        GetAlbumHandler getAlbumHandler,
        GetArtistAlbumsHandler getArtistAlbumsHandler,
        UpdateAlbumHandler updateAlbumHandler,
        DeleteAlbumHandler deleteAlbumHandler,
        PublishAlbumHandler publishAlbumHandler,
        UnpublishAlbumHandler unpublishAlbumHandler)
    {
        _createAlbumHandler = createAlbumHandler;
        _getAlbumHandler = getAlbumHandler;
        _getArtistAlbumsHandler = getArtistAlbumsHandler;
        _updateAlbumHandler = updateAlbumHandler;
        _deleteAlbumHandler = deleteAlbumHandler;
        _publishAlbumHandler = publishAlbumHandler;
        _unpublishAlbumHandler = unpublishAlbumHandler;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<AlbumResponse>> Create(
        [FromBody] CreateAlbumRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateAlbumCommand(
                request.ArtistId,
                request.Title);

            var album = await _createAlbumHandler.HandleAsync(
                command,
                cancellationToken);

            var response = new AlbumResponse(
                album.Id,
                album.ArtistId,
                album.Title,
                album.Description,
                album.CoverImageUrl,
                album.ReleaseDate,
                album.IsPublished,
                album.CreatedAt,
                album.UpdatedAt,
                []);

            return Created(
                $"/api/Albums/{album.Id}",
                response);
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

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AlbumResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var album = await _getAlbumHandler.HandleAsync(
            new GetAlbumQuery(id),
            cancellationToken);

        if (album is null)
        {
            return NotFound(new
            {
                message = "Album was not found."
            });
        }

        var response = new AlbumResponse(
            album.Id,
            album.ArtistId,
            album.Title,
            album.Description,
            album.CoverImageUrl,
            album.ReleaseDate,
            album.IsPublished,
            album.CreatedAt,
            album.UpdatedAt,
            album.Songs
                .OrderBy(song => song.TrackNumber)
                .Select(song => new AlbumSongResponse(
                    song.Id,
                    song.Title,
                    song.CoverImageUrl,
                    song.Duration,
                    song.ReleaseDate,
                    song.TrackNumber,
                    song.IsPublished))
                .ToList());

        return Ok(response);
    }

    [Authorize]
    [HttpGet("artist/{artistId:guid}")]
    public async Task<ActionResult<IEnumerable<AlbumResponse>>> GetByArtist(
        Guid artistId,
        CancellationToken cancellationToken)
    {
        var albums = await _getArtistAlbumsHandler.HandleAsync(
            new GetArtistAlbumsQuery(artistId),
            cancellationToken);

        var response = albums
            .Select(album => new AlbumResponse(
                album.Id,
                album.ArtistId,
                album.Title,
                album.Description,
                album.CoverImageUrl,
                album.ReleaseDate,
                album.IsPublished,
                album.CreatedAt,
                album.UpdatedAt,
                []))
            .ToList();

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AlbumResponse>> Update(
        Guid id,
        [FromBody] UpdateAlbumRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateAlbumCommand(
                id,
                request.Title,
                request.Description,
                request.CoverImageUrl,
                request.ReleaseDate);

            var album = await _updateAlbumHandler.HandleAsync(
                command,
                cancellationToken);

            var response = new AlbumResponse(
                album.Id,
                album.ArtistId,
                album.Title,
                album.Description,
                album.CoverImageUrl,
                album.ReleaseDate,
                album.IsPublished,
                album.CreatedAt,
                album.UpdatedAt,
                []);

            return Ok(response);
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

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _deleteAlbumHandler.HandleAsync(
                new DeleteAlbumCommand(id),
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

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/publish")]
    public async Task<ActionResult<AlbumResponse>> Publish(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var album = await _publishAlbumHandler.HandleAsync(
                new PublishAlbumCommand(id),
                cancellationToken);

            var response = new AlbumResponse(
                album.Id,
                album.ArtistId,
                album.Title,
                album.Description,
                album.CoverImageUrl,
                album.ReleaseDate,
                album.IsPublished,
                album.CreatedAt,
                album.UpdatedAt,
                []);

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

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/unpublish")]
    public async Task<ActionResult<AlbumResponse>> Unpublish(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var album = await _unpublishAlbumHandler.HandleAsync(
                new UnpublishAlbumCommand(id),
                cancellationToken);

            var response = new AlbumResponse(
                album.Id,
                album.ArtistId,
                album.Title,
                album.Description,
                album.CoverImageUrl,
                album.ReleaseDate,
                album.IsPublished,
                album.CreatedAt,
                album.UpdatedAt,
                []);

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
}