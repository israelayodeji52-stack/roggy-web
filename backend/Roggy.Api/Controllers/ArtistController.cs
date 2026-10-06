using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roggy.Application.Artists.CreateArtistProfile;
using Roggy.Application.Artists.GetArtistProfile;
using Roggy.Application.Artists.GetArtists;
using Roggy.Application.Artists.GetPopularArtists;
using Roggy.Application.Artists.SearchArtists;
using Roggy.Application.Artists.UnverifyArtist;
using Roggy.Application.Artists.UpdateArtistProfile;
using Roggy.Application.Artists.VerifyArtist;
using Roggy.Contracts.Artists;

namespace Roggy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly CreateArtistProfileHandler _createArtistProfileHandler;
    private readonly GetArtistProfileHandler _getArtistProfileHandler;
    private readonly GetArtistsHandler _getArtistsHandler;
    private readonly GetPopularArtistsHandler _getPopularArtistsHandler;
    private readonly SearchArtistsHandler _searchArtistsHandler;
    private readonly UpdateArtistProfileHandler _updateArtistProfileHandler;
    private readonly VerifyArtistHandler _verifyArtistHandler;
    private readonly UnverifyArtistHandler _unverifyArtistHandler;

    public ArtistsController(
        CreateArtistProfileHandler createArtistProfileHandler,
        GetArtistProfileHandler getArtistProfileHandler,
        GetArtistsHandler getArtistsHandler,
        GetPopularArtistsHandler getPopularArtistsHandler,
        SearchArtistsHandler searchArtistsHandler,
        UpdateArtistProfileHandler updateArtistProfileHandler,
        VerifyArtistHandler verifyArtistHandler,
        UnverifyArtistHandler unverifyArtistHandler)
    {
        _createArtistProfileHandler = createArtistProfileHandler;
        _getArtistProfileHandler = getArtistProfileHandler;
        _getArtistsHandler = getArtistsHandler;
        _getPopularArtistsHandler = getPopularArtistsHandler;
        _searchArtistsHandler = searchArtistsHandler;
        _updateArtistProfileHandler = updateArtistProfileHandler;
        _verifyArtistHandler = verifyArtistHandler;
        _unverifyArtistHandler = unverifyArtistHandler;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ArtistProfileResponse>> Create(
        [FromBody] CreateArtistProfileRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateArtistProfileCommand(
                request.UserId,
                request.StageName);

            var artistProfile =
                await _createArtistProfileHandler.HandleAsync(
                    command,
                    cancellationToken);

            var response = new ArtistProfileResponse(
                artistProfile.Id,
                artistProfile.UserId,
                artistProfile.StageName,
                artistProfile.Biography,
                artistProfile.ProfileImageUrl,
                artistProfile.BannerImageUrl,
                artistProfile.IsVerified,
                artistProfile.CreatedAt,
                artistProfile.UpdatedAt);

            return CreatedAtAction(
                nameof(GetById),
                new { id = artistProfile.Id },
                response);
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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistProfileResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var artists =
            await _getArtistsHandler.HandleAsync(
                new GetArtistsQuery(),
                cancellationToken);

        var response = artists
            .Select(artist => new ArtistProfileResponse(
                artist.Id,
                artist.UserId,
                artist.StageName,
                artist.Biography,
                artist.ProfileImageUrl,
                artist.BannerImageUrl,
                artist.IsVerified,
                artist.CreatedAt,
                artist.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [Authorize]
    [HttpGet("popular")]
    public async Task<ActionResult<IEnumerable<PopularArtistResponse>>> GetPopular(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var artists =
            await _getPopularArtistsHandler.HandleAsync(
                new GetPopularArtistsQuery(limit),
                cancellationToken);

        var response = artists
            .Select(artist => new PopularArtistResponse(
                artist.Id,
                artist.UserId,
                artist.StageName,
                artist.Biography,
                artist.ProfileImageUrl,
                artist.BannerImageUrl,
                artist.IsVerified,
                artist.CreatedAt,
                artist.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ArtistProfileResponse>>> Search(
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

        var artists =
            await _searchArtistsHandler.HandleAsync(
                new SearchArtistsQuery(query),
                cancellationToken);

        var response = artists
            .Select(artist => new ArtistProfileResponse(
                artist.Id,
                artist.UserId,
                artist.StageName,
                artist.Biography,
                artist.ProfileImageUrl,
                artist.BannerImageUrl,
                artist.IsVerified,
                artist.CreatedAt,
                artist.UpdatedAt))
            .ToList();

        return Ok(response);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ArtistProfileResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var artistProfile =
                await _getArtistProfileHandler.HandleAsync(
                    new GetArtistProfileQuery(id),
                    cancellationToken);

            var response = new ArtistProfileResponse(
                artistProfile.Id,
                artistProfile.UserId,
                artistProfile.StageName,
                artistProfile.Biography,
                artistProfile.ProfileImageUrl,
                artistProfile.BannerImageUrl,
                artistProfile.IsVerified,
                artistProfile.CreatedAt,
                artistProfile.UpdatedAt);

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
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ArtistProfileResponse>> Update(
        Guid id,
        [FromBody] UpdateArtistProfileRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateArtistProfileCommand(
                id,
                request.StageName,
                request.Biography,
                request.ProfileImageUrl,
                request.BannerImageUrl);

            var artistProfile =
                await _updateArtistProfileHandler.HandleAsync(
                    command,
                    cancellationToken);

            var response = new ArtistProfileResponse(
                artistProfile.Id,
                artistProfile.UserId,
                artistProfile.StageName,
                artistProfile.Biography,
                artistProfile.ProfileImageUrl,
                artistProfile.BannerImageUrl,
                artistProfile.IsVerified,
                artistProfile.CreatedAt,
                artistProfile.UpdatedAt);

            return Ok(response);
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

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/verify")]
    public async Task<ActionResult<ArtistProfileResponse>> Verify(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var artistProfile =
                await _verifyArtistHandler.HandleAsync(
                    new VerifyArtistCommand(id),
                    cancellationToken);

            var response = new ArtistProfileResponse(
                artistProfile.Id,
                artistProfile.UserId,
                artistProfile.StageName,
                artistProfile.Biography,
                artistProfile.ProfileImageUrl,
                artistProfile.BannerImageUrl,
                artistProfile.IsVerified,
                artistProfile.CreatedAt,
                artistProfile.UpdatedAt);

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
    [HttpPost("{id:guid}/unverify")]
    public async Task<ActionResult<ArtistProfileResponse>> Unverify(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var artistProfile =
                await _unverifyArtistHandler.HandleAsync(
                    new UnverifyArtistCommand(id),
                    cancellationToken);

            var response = new ArtistProfileResponse(
                artistProfile.Id,
                artistProfile.UserId,
                artistProfile.StageName,
                artistProfile.Biography,
                artistProfile.ProfileImageUrl,
                artistProfile.BannerImageUrl,
                artistProfile.IsVerified,
                artistProfile.CreatedAt,
                artistProfile.UpdatedAt);

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